// results.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NgbProgressbarModule } from '@ng-bootstrap/ng-bootstrap';
import { InterviewResultDto } from '../../models/interview.model';

@Component({
  selector: 'app-results',
  standalone: true,
  imports: [CommonModule, NgbProgressbarModule],
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
export class ResultsComponent implements OnInit {
  result: InterviewResultDto | null = null;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.result = navigation?.extras?.state?.['result'] || null;
  }

  ngOnInit(): void {
    if (!this.result) {
      this.router.navigate(['/']);
    }
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
}
