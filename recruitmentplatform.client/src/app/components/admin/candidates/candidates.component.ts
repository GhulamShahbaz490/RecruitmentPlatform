import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminApiService } from '../../../services/admin-api.service';

@Component({
  selector: 'app-admin-candidates',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './candidates.component.html'
})
export class CandidatesComponent implements OnInit {
  candidates: any[] = [];
  loading = false;
  search = '';
  statusFilter = '';
  minPercentage: number | null = null;

  constructor(private adminApi: AdminApiService) {}

  ngOnInit(): void {
    this.loadCandidates();
  }

  loadCandidates(): void {
    this.loading = true;
    this.adminApi.getCandidates({ status: this.statusFilter || undefined, minPercentage: this.minPercentage ?? undefined, search: this.search || undefined })
      .subscribe({ next: (res) => { this.candidates = res; this.loading = false; }, error: () => { this.loading = false; } });
  }

  clearFilters(): void {
    this.search = '';
    this.statusFilter = '';
    this.minPercentage = null;
    this.loadCandidates();
  }
}
