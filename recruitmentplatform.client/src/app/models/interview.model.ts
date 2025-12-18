export interface InterviewQuestionDto {
  id: string;
  section: string;
  text: string;
  options: string[];
  timeLimitSeconds: number;
}

export interface SubmitAnswerDto {
  questionId: string;
  selectedAnswerIndex: number;
  timeTakenSeconds: number;
}

export interface InterviewResultDto {
  interviewNumber: string;
  applicantName: string;
  percentage: number;
  status: string;
  totalScore: number;
  maxPossibleScore: number;
  sectionScores: { [key: string]: SectionScoreDto };
}

export interface SectionScoreDto {
  score: number;
  maxScore: number;
  percentage: number;
}