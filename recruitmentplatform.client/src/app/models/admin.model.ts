export interface AdminLoginDto {
  email: string;
  password: string;
}

export interface QuestionManagementDto {
  id?: string;
  section: string;
  text: string;
  options: string[];
  correctAnswerIndex: number;
  points: number;
  explanation: string;
}

export interface SettingsDto {
  passThresholdPercentage: number;
  questionTimeLimitSeconds: number;
  totalQuestionsPerSection: number;
  randomizeQuestions: boolean;
  emailNotificationsEnabled: boolean;
}

export interface PositionDto {
  id?: string;
  title: string;
  description: string;
  level: number;
  techStack: string;
  location: string;
  isActive: boolean;
}

export type ExperienceLevel = 'Junior' | 'MidLevel' | 'Senior' | 'Lead';
export type QuestionSection = 'Basic' | 'Technical' | 'Practical';
