import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
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
  SectionScoreDto
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
    return this.http.post<any>(
      `${this.baseUrl}/interview/start`,
      {}
    ).pipe(map(response => ({ sessionId: response?.SessionId || response?.sessionId })));
  }

  getNextQuestion(sessionId: string): Observable<InterviewQuestionDto | null> {
    return this.http.get<any>(
      `${this.baseUrl}/interview/next-question`,
      {
        params: { sessionId },
      }
    ).pipe(map(response => {
      // Server may return an object like { Message: 'Interview completed or no more questions' }
      if (!response) return null;
      if (response.message || response.Message) return null;
      return response as InterviewQuestionDto;
    }));
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

  private normalizeResult(response: any): InterviewResultDto {
    if (!response) return {
      interviewNumber: '',
      applicantName: '',
      percentage: 0,
      status: '',
      totalScore: 0,
      maxPossibleScore: 0,
      sectionScores: {}
    };

    const raw = response.sectionScores ?? response.SectionScores ?? {};
    const sectionScores: { [key: string]: SectionScoreDto } = {};
    Object.keys(raw).forEach(k => {
      const s = raw[k];
      sectionScores[k] = {
        score: s?.score ?? s?.Score ?? 0,
        maxScore: s?.maxScore ?? s?.MaxScore ?? 0,
        percentage: s?.percentage ?? s?.Percentage ?? 0
      };
    });

    return {
      interviewNumber: response.interviewNumber ?? response.InterviewNumber ?? '',
      applicantName: response.applicantName ?? response.ApplicantName ?? '',
      percentage: response.percentage ?? response.Percentage ?? 0,
      status: response.status ?? response.Status ?? '',
      totalScore: response.totalScore ?? response.TotalScore ?? 0,
      maxPossibleScore: response.maxPossibleScore ?? response.MaxPossibleScore ?? 0,
      sectionScores
    };
  }

  completeInterview(sessionId: string): Observable<InterviewResultDto> {
    return this.http.post<any>(
      `${this.baseUrl}/interview/complete?sessionId=${sessionId}`,
      {}
    ).pipe(map(response => this.normalizeResult(response)));
  }

  getResults(sessionId: string): Observable<InterviewResultDto> {
    return this.http.get<any>(
      `${this.baseUrl}/interview/results`,
      {
        params: { sessionId },
      }
    ).pipe(map(response => this.normalizeResult(response)));
  }

  getMyApplications(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/applications/my-applications`).pipe(
      map(apps => apps.map(a => ({
        ...a,
        latestSessionId: a.latestSessionId || a.LatestSessionId || null,
        positionTitle: a.positionTitle || a.PositionTitle || a.positionTitle,
        techStack: a.techStack || a.TechStack || a.techStack,
        positionId:
          a.positionId ||
          a.PositionId ||
          a.positionID ||
          a.PositionID ||
          a.position?.id ||
          a.Position?.Id ||
          null
      })))
    );
  }
}
