// results.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbProgressbarModule } from '@ng-bootstrap/ng-bootstrap';
import { InterviewResultDto } from '../../models/interview.model';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-results',
  standalone: true,
  imports: [CommonModule, NgbProgressbarModule],
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
export class ResultsComponent implements OnInit {
  result: InterviewResultDto | null = null;
  emailSent = true; // Email is sent in CompleteInterview endpoint
  loading = false;

  constructor(private router: Router, private route: ActivatedRoute, private apiService: ApiService) {
    const navigation = this.router.getCurrentNavigation?.();
    this.result = navigation?.extras?.state?.['result'] || null;
  }

  ngOnInit(): void {
    if (this.result) return;

    // Try to read sessionId from query params and fetch results
    this.route.queryParams.subscribe(params => {
      const sessionId = params['sessionId'];
      if (!sessionId) {
        this.router.navigate(['/']);
        return;
      }

      this.loading = true;
      this.apiService.getResults(sessionId).subscribe({
        next: (res) => {
          this.result = res as InterviewResultDto;
          this.loading = false;
          if (!this.result) this.router.navigate(['/']);
        },
        error: () => {
          this.loading = false;
          this.router.navigate(['/']);
        }
      });
    });
  }

  getStatusColor(): string {
    return this.result?.status === 'Pass' ? '#4CAF50' : '#f44336';
  }

  getSectionKeys(): string[] {
    return this.result ? Object.keys(this.result.sectionScores) : [];
  }

  goHome(): void {
    this.router.navigate(['/']);
  }

  getApplicantEmail(): string {
    // Extract email from applicant name or use a placeholder
    // The email should ideally come from the result object
    return 'your registered email';
  }
}
