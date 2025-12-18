using RecruitmentPlatform.Server.Data;
using RecruitmentPlatform.Server.DTOs;
using RecruitmentPlatform.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentPlatform.Server.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InterviewService> _logger;

        public InterviewService(ApplicationDbContext context, ILogger<InterviewService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<InterviewSession> StartInterviewAsync(Guid applicationId)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null)
                throw new ArgumentException("Application not found");

            // Use a serializable transaction to avoid race conditions where two requests
            // create two in-progress sessions for the same application concurrently.
            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                // Re-check for existing in-progress session inside the transaction
                var existingSession = await _context.InterviewSessions
                    .FirstOrDefaultAsync(s => s.ApplicationId == applicationId && s.Status == InterviewStatus.InProgress);

                if (existingSession != null)
                {
                    await transaction.CommitAsync();
                    return existingSession;
                }

                var session = new InterviewSession
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = applicationId,
                    Status = InterviewStatus.InProgress,
                    StartedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    MaxPossibleScore = await _context.Questions.SumAsync(q => q.Points)
                };

                _context.InterviewSessions.Add(session);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation($"Interview started for application {applicationId}");
                return session;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<InterviewQuestionDto> GetNextQuestionAsync(Guid interviewSessionId)
        {
            var session = await _context.InterviewSessions
                .Include(s => s.Answers)
                .FirstOrDefaultAsync(s => s.Id == interviewSessionId);

            if (session == null || session.Status != InterviewStatus.InProgress)
                return null;

            // Determine current section based on answered questions
            var answeredQuestions = session.Answers.Select(a => a.QuestionId).ToList();
            var questions = await _context.Questions
                .Where(q => !answeredQuestions.Contains(q.Id))
                .OrderBy(q => q.Section)
                .ToListAsync();

            var nextQuestion = questions.FirstOrDefault();
            if (nextQuestion == null)
                return null;

            return new InterviewQuestionDto
            {
                Id = nextQuestion.Id,
                Section = nextQuestion.Section.ToString(),
                Text = nextQuestion.Text,
                Options = nextQuestion.Options,
                TimeLimitSeconds = 60
            };
        }

        public async Task<bool> SubmitAnswerAsync(Guid interviewSessionId, SubmitAnswerDto answerDto)
        {
            var session = await _context.InterviewSessions
                .Include(s => s.Answers)
                .FirstOrDefaultAsync(s => s.Id == interviewSessionId);

            if (session == null || session.Status != InterviewStatus.InProgress)
                throw new InvalidOperationException("Invalid session");

            var question = await _context.Questions.FindAsync(answerDto.QuestionId);
            if (question == null)
                throw new ArgumentException("Question not found");

            // Check if already answered
            if (session.Answers.Any(a => a.QuestionId == answerDto.QuestionId))
                throw new InvalidOperationException("Question already answered");

            var isCorrect = answerDto.SelectedAnswerIndex == question.CorrectAnswerIndex;

            var answer = new Answer
            {
                InterviewSessionId = interviewSessionId,
                QuestionId = answerDto.QuestionId,
                SelectedAnswerIndex = answerDto.SelectedAnswerIndex,
                IsCorrect = isCorrect,
                TimeTakenSeconds = answerDto.TimeTakenSeconds
            };

            // Add to the session's navigation collection so in-memory state stays consistent
            session.Answers.Add(answer);
            // Also add to DbSet to ensure it will be persisted
            _context.Answers.Add(answer);

            if (isCorrect)
                session.TotalScore += question.Points;

            await _context.SaveChangesAsync();

            // Check if interview is complete
            var totalQuestions = await _context.Questions.CountAsync();
            // Use DB count for answers to ensure recently added answer is counted reliably
            var answeredCount = await _context.Answers.CountAsync(a => a.InterviewSessionId == interviewSessionId);

            if (answeredCount >= totalQuestions)
            {
                await CompleteInterviewAsync(interviewSessionId);
            }

            return isCorrect;
        }

        public async Task<InterviewResultDto> CompleteInterviewAsync(Guid interviewSessionId)
        {
            var session = await _context.InterviewSessions
                .Include(s => s.Application)
                .Include(s => s.Answers)
                .ThenInclude(a => a.Question)
                .FirstOrDefaultAsync(s => s.Id == interviewSessionId);

            if (session == null)
                return null;

            // If already completed, return existing result to avoid duplicate entries
            var existingResult = await _context.Results.FirstOrDefaultAsync(r => r.InterviewSessionId == interviewSessionId);
            if (existingResult != null)
            {
                return new InterviewResultDto
                {
                    InterviewNumber = session.Application.InterviewNumber,
                    ApplicantName = $"{session.Application.FirstName} {session.Application.LastName}",
                    Percentage = existingResult.Percentage,
                    Status = existingResult.Status.ToString(),
                    TotalScore = existingResult.TotalScore,
                    MaxPossibleScore = existingResult.MaxPossibleScore,
                    SectionScores = session.Answers
                        .GroupBy(a => a.Question.Section)
                        .ToDictionary(
                            g => g.Key.ToString(),
                            g => new SectionScoreDto
                            {
                                Score = g.Where(a => a.IsCorrect).Sum(a => a.Question.Points),
                                MaxScore = g.Sum(a => a.Question.Points),
                                Percentage = g.Sum(a => a.Question.Points) > 0
                                    ? (double)g.Where(a => a.IsCorrect).Sum(a => a.Question.Points) / g.Sum(a => a.Question.Points) * 100
                                    : 0
                            })
                };
            }

            session.Status = InterviewStatus.Completed;
            session.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Calculate detailed results
            var sectionScores = session.Answers
                .GroupBy(a => a.Question.Section)
                .ToDictionary(
                    g => g.Key.ToString(),
                    g => new SectionScoreDto
                    {
                        Score = g.Where(a => a.IsCorrect).Sum(a => a.Question.Points),
                        MaxScore = g.Sum(a => a.Question.Points),
                        Percentage = g.Sum(a => a.Question.Points) > 0
                            ? (double)g.Where(a => a.IsCorrect).Sum(a => a.Question.Points) / g.Sum(a => a.Question.Points) * 100
                            : 0
                    });

            var percentage = session.MaxPossibleScore > 0 ? (double)session.TotalScore / session.MaxPossibleScore * 100 : 0;
            var status = percentage >= 70 ? ResultStatus.Pass : ResultStatus.Fail;

            var result = new Result
            {
                InterviewSessionId = interviewSessionId,
                TotalScore = session.TotalScore,
                MaxPossibleScore = session.MaxPossibleScore,
                Percentage = Math.Round(percentage, 2),
                Status = status
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Interview completed for {session.Application.Email} with score {percentage:F1}%");

            return new InterviewResultDto
            {
                InterviewNumber = session.Application.InterviewNumber,
                ApplicantName = $"{session.Application.FirstName} {session.Application.LastName}",
                Percentage = result.Percentage,
                Status = status.ToString(),
                TotalScore = result.TotalScore,
                MaxPossibleScore = result.MaxPossibleScore,
                SectionScores = sectionScores
            };
        }
    }
}
