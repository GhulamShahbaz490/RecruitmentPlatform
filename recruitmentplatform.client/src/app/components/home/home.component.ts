// home.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Position } from '../../models/position.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  positions: Position[] = [];
  myApplications: any[] = [];
  loading = true;
  isLoggedIn = false;

  constructor(
    private apiService: ApiService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isAuthenticated();

    // If query param 'view=positions' is present, show positions even when logged in
    this.route.queryParams.subscribe((params) => {
      const view = params['view'];
      if (view === 'positions') {
        this.loadPositions();
        return;
      }
      // Default behaviour
      if (this.isLoggedIn) {
        this.loadMyApplications();
      } else {
        this.loadPositions();
      }
    });
  }

  loadPositions(): void {
    this.loading = true;
    this.apiService.getPositions().subscribe({
      next: (positions) => {
        this.positions = positions;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load positions', err);
        this.loading = false;
      },
    });
  }

  loadMyApplications(): void {
    this.loading = true;
    this.apiService.getMyApplications().subscribe({
      next: (applications) => {
        this.myApplications = applications;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load applications', err);
        this.loading = false;
      },
    });
  }

  startInterview(sessionId: string): void {
    if (sessionId) {
      this.router.navigate(['/interview'], { queryParams: { sessionId } });
    } else {
      // Create new interview session
      this.apiService.startInterview().subscribe({
        next: (response: any) => {
          // Accept both camelCase and PascalCase from backend
          const sid =
            response?.sessionId ||
            response?.SessionId ||
            response?.SessionID ||
            response?.sessionID;
          if (sid) {
            this.router.navigate(['/interview'], { queryParams: { sessionId: sid } });
          } else {
            // Fallback: navigate to interview without id
            this.router.navigate(['/interview']);
          }
        },
        error: () => {
          // Show error with ng-bootstrap alert
        },
      });
    }
  }

  getTechStackList(techStack: string): string[] {
    return techStack ? techStack.split(',').map((s) => s.trim()) : [];
  }
}
