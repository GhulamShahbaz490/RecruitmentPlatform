using Microsoft.EntityFrameworkCore;
using RecruitmentPlatform.Server.Models;

namespace RecruitmentPlatform.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<InterviewSession> InterviewSessions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<InterviewSettings> InterviewSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed sample questions
            modelBuilder.Entity<Question>().HasData(
                // Basic Questions
                new Question
                {
                    Id = Guid.NewGuid(),
                    Section = QuestionSection.Basic,
                    Text = "What is the primary purpose of the .NET Framework?",
                    Options = new List<string> { "Web development", "Multiple language interoperability", "Database management", "Mobile apps" },
                    CorrectAnswerIndex = 1,
                    Points = 10,
                    Explanation = ".NET provides a common runtime for multiple languages."
                },
                new Question
                {
                    Id = Guid.NewGuid(),
                    Section = QuestionSection.Basic,
                    Text = "Which of the following is a key principle of Agile development?",
                    Options = new List<string> { "Detailed documentation", "Customer collaboration", "Contract negotiation", "Rigid processes" },
                    CorrectAnswerIndex = 1,
                    Points = 10,
                    Explanation = "Agile values customer collaboration over contract negotiation."
                },

                // Technical Questions
                new Question
                {
                    Id = Guid.NewGuid(),
                    Section = QuestionSection.Technical,
                    Text = "What is the difference between '==' and 'Equals()' in C#?",
                    Options = new List<string> { "No difference", "== checks reference, Equals checks value", "== checks value, Equals checks reference", "Depends on the type" },
                    CorrectAnswerIndex = 3,
                    Points = 15,
                    Explanation = "Behavior differs between value types and reference types."
                },
                new Question
                {
                    Id = Guid.NewGuid(),
                    Section = QuestionSection.Technical,
                    Text = "Which LINQ method is used for deferred execution?",
                    Options = new List<string> { "ToList()", "First()", "Where()", "Count()" },
                    CorrectAnswerIndex = 2,
                    Points = 15,
                    Explanation = "Where() returns IEnumerable and uses deferred execution."
                },

                // Practical Questions
                new Question
                {
                    Id = Guid.NewGuid(),
                    Section = QuestionSection.Practical,
                    Text = "A production API is returning 500 errors sporadically. First step?",
                    Options = new List<string> { "Restart server", "Check logs", "Scale up instances", "Notify all users" },
                    CorrectAnswerIndex = 1,
                    Points = 20,
                    Explanation = "Logs provide diagnostic information for root cause analysis."
                },
                new Question
                {
                    Id = Guid.NewGuid(),
                    Section = QuestionSection.Practical,
                    Text = "How to optimize a slow database query in Entity Framework?",
                    Options = new List<string> { "Use AsNoTracking()", "Increase timeout", "Add more indexes", "All of the above" },
                    CorrectAnswerIndex = 3,
                    Points = 20,
                    Explanation = "Multiple strategies can be combined for query optimization."
                }
            );

            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@recruit.com",
                    PasswordHash = "Admin@123",
                    FullName = "System Administrator",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed default settings
            modelBuilder.Entity<InterviewSettings>().HasData(
                new InterviewSettings
                {
                    Id = Guid.NewGuid(),
                    PassThresholdPercentage = 70,
                    QuestionTimeLimitSeconds = 60,
                    TotalQuestionsPerSection = 5,
                    RandomizeQuestions = true,
                    EmailNotificationsEnabled = true,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
