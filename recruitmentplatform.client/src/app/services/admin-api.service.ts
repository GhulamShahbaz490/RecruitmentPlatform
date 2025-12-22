import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  AdminLoginDto, 
  QuestionManagementDto, 
  SettingsDto, 
  PositionDto 
} from '../models/admin.model';
import { Position } from '../models/position.model';

@Injectable({
  providedIn: 'root'
})
export class AdminApiService {
  private baseUrl = 'https://localhost:7000/api/admin';

  constructor(private http: HttpClient) {}

  // Auth
  login(dto: AdminLoginDto): Observable<{ token: string; email: string; fullName: string }> {
    return this.http.post<{ token: string; email: string; fullName: string }>(
      `${this.baseUrl}/auth/login`, dto
    );
  }

  // Positions
  getAllPositions(): Observable<Position[]> {
    return this.http.get<Position[]>(`${this.baseUrl}/positions`);
  }

  createPosition(position: PositionDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/positions`, position);
  }

  updatePosition(id: string, position: PositionDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/positions/${id}`, position);
  }

  deletePosition(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/positions/${id}`);
  }

  // Questions
  getAllQuestions(): Observable<QuestionManagementDto[]> {
    return this.http.get<QuestionManagementDto[]>(`${this.baseUrl}/questions`);
  }

  createQuestion(question: QuestionManagementDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/questions`, question);
  }

  updateQuestion(id: string, question: QuestionManagementDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/questions/${id}`, question);
  }

  deleteQuestion(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/questions/${id}`);
  }

  // Settings
  getSettings(): Observable<SettingsDto> {
    return this.http.get<SettingsDto>(`${this.baseUrl}/settings`);
  }

  updateSettings(settings: SettingsDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/settings`, settings);
  }

  // Candidates
  getCandidates(params?: { status?: string; minPercentage?: number; search?: string }): Observable<any[]> {
    const query: any = {};
    if (params?.status) query.status = params.status;
    if (params?.minPercentage !== undefined) query.minPercentage = String(params.minPercentage);
    if (params?.search) query.search = params.search;
    return this.http.get<any[]>(`${this.baseUrl}/candidates`, { params: query });
  }
}
