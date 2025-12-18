// dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminApiService } from '../../../services/admin-api.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  stats = {
    totalPositions: 0,
    totalQuestions: 0,
    activeApplications: 0,
    avgPassRate: 0
  };

  constructor(private adminApiService: AdminApiService) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    // Load various stats
    this.adminApiService.getAllPositions().subscribe(positions => {
      this.stats.totalPositions = positions.length;
    });

    this.adminApiService.getAllQuestions().subscribe(questions => {
      this.stats.totalQuestions = questions.length;
    });

    // Add more stats as needed
  }
}