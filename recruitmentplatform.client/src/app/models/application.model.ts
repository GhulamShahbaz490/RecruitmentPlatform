export interface RegisterApplicationDto {
  positionId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  resume: File;
}

export interface LoginDto {
  email: string;
  pinCode: string;
}

export interface AuthResponse {
  token: string;
  interviewNumber: string;
}