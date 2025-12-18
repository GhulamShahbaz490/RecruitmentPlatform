import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Position } from '../models/position.model';
import {
  RegisterApplicationDto,
  LoginDto,
  AuthResponse,
} from '../models/application.model';
import {
  InterviewQuestionDto,
  SubmitAnswerDto,
  InterviewResultDto,
} from '../models/interview.model';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = 'https://localhost:7000/api'; // Update with your API URL

  constructor(private http: HttpClient) {}

  // Positions
  getPositions(): Observable<Position[]> {
    return this.http.get<Position[]>(`${this.baseUrl}/positions`);
  }

  getPosition(id: string): Observable<Position> {
    return this.http.get<Position>(`${this.baseUrl}/positions/${id}`);
  }

  // Applications
  apply(dto: FormData): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.baseUrl}/applications/apply`,
      dto
    );
  }

  // Auth
  login(dto: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/auth/login`, dto);
  }

  // Interview
  startInterview(): Observable<{ sessionId: string }> {
    return this.http.post<{ sessionId: string }>(
      `${this.baseUrl}/interview/start`,
      {}
    );
  }

  getNextQuestion(sessionId: string): Observable<InterviewQuestionDto> {
    return this.http.get<InterviewQuestionDto>(
      `${this.baseUrl}/interview/next-question`,
      {
        params: { sessionId },
      }
    );
  }

  submitAnswer(
    sessionId: string,
    dto: SubmitAnswerDto
  ): Observable<{ isCorrect: boolean }> {
    return this.http.post<{ isCorrect: boolean }>(
      `${this.baseUrl}/interview/submit-answer?sessionId=${sessionId}`,
      dto
    );
  }

  completeInterview(sessionId: string): Observable<InterviewResultDto> {
    return this.http.post<InterviewResultDto>(
      `${this.baseUrl}/interview/complete?sessionId=${sessionId}`,
      {}
    );
  }

  getResults(sessionId: string): Observable<InterviewResultDto> {
    return this.http.get<InterviewResultDto>(
      `${this.baseUrl}/interview/results`,
      {
        params: { sessionId },
      }
    );
  }

  getMyApplications(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/applications/my-applications`);
  }
}
