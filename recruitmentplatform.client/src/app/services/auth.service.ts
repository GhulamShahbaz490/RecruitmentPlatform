import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from './api.service';
import { LoginDto, AuthResponse } from '../models/application.model';
import { AdminLoginDto } from '../models/admin.model';
import { AdminApiService } from './admin-api.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private tokenKey = 'auth-token';
  private interviewNumberKey = 'interview-number';
  private adminTokenKey = 'admin-token';
  private adminNameKey = 'admin-name';
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(null);

  constructor(private apiService: ApiService, private adminApiService: AdminApiService) {
    this.loadStoredAuth();
  }

  private loadStoredAuth(): void {
    const token = localStorage.getItem(this.tokenKey);
    const interviewNumber = localStorage.getItem(this.interviewNumberKey);
    if (token && interviewNumber) {
      this.currentUserSubject.next({ token, interviewNumber });
    }
  }

  login(dto: LoginDto): Observable<AuthResponse> {
    return new Observable((observer) => {
      this.apiService.login(dto).subscribe({
        next: (response) => {
          this.storeAuth(response);
          observer.next(response);
          observer.complete();
        },
        error: (err) => observer.error(err),
      });
    });
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.interviewNumberKey);
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getInterviewNumber(): string | null {
    return localStorage.getItem(this.interviewNumberKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private storeAuth(response: AuthResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.interviewNumberKey, response.interviewNumber);
    this.currentUserSubject.next(response);
  }

  // Public setter to store auth response (used when API returns token on apply)
  setAuth(response: AuthResponse): void {
    this.storeAuth(response);
  }

  get currentUser$(): Observable<AuthResponse | null> {
    return this.currentUserSubject.asObservable();
  }

  loginAdmin(
    dto: AdminLoginDto
  ): Observable<{ token: string; email: string; fullName: string }> {
    return new Observable((observer) => {
      this.adminApiService.login(dto).subscribe({
        next: (response) => {
          localStorage.setItem(this.adminTokenKey, response.token);
          localStorage.setItem(this.adminNameKey, response.fullName);
          this.currentUserSubject.next({
            token: response.token,
            interviewNumber: 'ADMIN',
          });
          observer.next(response);
          observer.complete();
        },
        error: (err) => observer.error(err),
      });
    });
  }

  getAdminToken(): string | null {
    return localStorage.getItem(this.adminTokenKey);
  }

  getAdminName(): string | null {
    return localStorage.getItem(this.adminNameKey);
  }

  isAdmin(): boolean {
    return !!this.getAdminToken();
  }

  logoutAdmin(): void {
    localStorage.removeItem(this.adminTokenKey);
    localStorage.removeItem(this.adminNameKey);
    this.currentUserSubject.next(null);
  }
}
