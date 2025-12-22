// home.component.ts
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { ApiService } from '../../services/api.service';
import { Position } from '../../models/position.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  positions: Position[] = [];
  myApplications: any[] = [];
  loading = true;
  isLoggedIn = false;

  // Browse Positions Modal
  @ViewChild('browsePositionsModal') browsePositionsModal!: TemplateRef<any>;
  private browseModalRef?: NgbModalRef;

  browseLoading = false;
  browseError: string | null = null;
  browseSearch = '';
  availablePositions: Position[] = [];

  constructor(
    private apiService: ApiService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private modal: NgbModal
  ) { }

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isAuthenticated();

    // React to auth changes so UI updates immediately after login/apply
    this.authService.currentUser$.subscribe((u) => {
      const wasLogged = this.isLoggedIn;
      this.isLoggedIn = !!u;
      if (this.isLoggedIn && !wasLogged) {
        // switched to logged in: load applications
        this.loadMyApplications();
      } else if (!this.isLoggedIn && wasLogged) {
        // logged out: load positions for guests
        this.loadPositions();
      }
    });

    // If query param 'view=positions' is present, show positions even when logged in
    this.route.queryParams.subscribe((params) => {
      const view = params['view'];
      if (view === 'positions') {
        this.loadPositions();
        return;
      }

      if (this.isLoggedIn) {
        this.loadMyApplications();
      } else {
        this.loadPositions();
      }
    });
  }

  loadPositions(): void {
    this.loading = true;
    // If user is logged in, exclude positions they already applied for
    if (!this.authService.isAuthenticated()) {
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
      return;
    }

    // When authenticated, load user's applications first and filter out applied positions
    this.apiService.getMyApplications().subscribe({
      next: (applications) => {
        const appliedIds = new Set<any>();
        for (const a of applications || []) {
          const pid = a?.positionId ?? a?.PositionId ?? a?.position?.id ?? a?.Position?.Id;
          if (pid !== undefined && pid !== null) appliedIds.add(pid?.toString());
        }

        this.apiService.getPositions().subscribe({
          next: (positions) => {
            this.positions = (positions || []).filter((p: any) => {
              const pid = p?.id ?? p?.Id;
              return pid != null && !appliedIds.has(pid.toString());
            });
            this.loading = false;
          },
          error: (err) => {
            console.error('Failed to load positions', err);
            this.loading = false;
          }
        });
      },
      error: (err) => {
        // Fallback: load positions if we couldn't fetch applications
        console.error('Failed to load applications for filtering', err);
        this.apiService.getPositions().subscribe({
          next: (positions) => {
            this.positions = positions;
            this.loading = false;
          },
          error: (err2) => {
            console.error('Failed to load positions', err2);
            this.loading = false;
          }
        });
      }
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

  // Opens an ng-bootstrap modal, shows only positions NOT applied yet
  openBrowseMorePositions(): void {
    if (!this.isLoggedIn) {
      this.router.navigate(['/positions']);
      return;
    }

    this.browseLoading = true;
    this.browseError = null;
    this.browseSearch = '';
    this.availablePositions = [];

    // Fetch user's applications first to have an up-to-date list
    this.apiService.getMyApplications().subscribe({
      next: (applications) => {
        const appliedIds = new Set<string>();
        for (const a of applications || []) {
          const pid = a?.positionId ?? a?.PositionId ?? a?.position?.id ?? a?.Position?.Id;
          if (pid !== undefined && pid !== null) appliedIds.add(String(pid));
        }

        this.apiService.getPositions().subscribe({
          next: (allPositions) => {
            this.availablePositions = (allPositions || []).filter((p: any) => {
              const pid = p?.id ?? p?.Id;
              return pid != null && !appliedIds.has(String(pid));
            });

            this.browseLoading = false;
            this.browseModalRef = this.modal.open(this.browsePositionsModal, {
              size: 'lg',
              centered: true,
              scrollable: true,
              backdrop: 'static',
            });
          },
          error: (err) => {
            console.error('Failed to load positions for modal', err);
            this.browseLoading = false;
            this.browseError = 'Unable to load positions right now.';
            this.browseModalRef = this.modal.open(this.browsePositionsModal, {
              size: 'lg',
              centered: true,
              scrollable: true,
              backdrop: 'static',
            });
          }
        });
      },
      error: (err) => {
        console.error('Failed to load applications for modal filtering', err);
        // Fallback to loading positions alone
        this.apiService.getPositions().subscribe({
          next: (allPositions) => {
            this.availablePositions = allPositions || [];
            this.browseLoading = false;
            this.browseModalRef = this.modal.open(this.browsePositionsModal, {
              size: 'lg',
              centered: true,
              scrollable: true,
              backdrop: 'static',
            });
          },
          error: (err2) => {
            console.error('Failed to load positions for modal', err2);
            this.browseLoading = false;
            this.browseError = 'Unable to load positions right now.';
            this.browseModalRef = this.modal.open(this.browsePositionsModal, {
              size: 'lg',
              centered: true,
              scrollable: true,
              backdrop: 'static',
            });
          }
        });
      }
    });
  }

  get filteredAvailablePositions(): Position[] {
    const q = (this.browseSearch || '').trim().toLowerCase();
    if (!q) return this.availablePositions;

    return (this.availablePositions || []).filter((p: any) => {
      const title = (p?.title ?? '').toLowerCase();
      const desc = (p?.description ?? '').toLowerCase();
      const tech = (p?.techStack ?? '').toLowerCase();
      const level = (p?.level ?? '').toLowerCase();
      const loc = (p?.location ?? '').toLowerCase();
      return (title + ' ' + desc + ' ' + tech + ' ' + level + ' ' + loc).includes(q);
    });
  }

  // Keeps your existing flow: navigate to /apply/:id
  pickPosition(position: Position): void {
    const id: any = (position as any)?.id ?? (position as any)?.Id;
    if (!id) return;

    this.browseModalRef?.close();
    this.router.navigate(['/apply', id]);
  }

  private getAppliedPositionIds(): Set<any> {
    const ids = new Set<any>();
    for (const a of this.myApplications || []) {
      const pid =
        a?.positionId ??
        a?.PositionId ??
        a?.positionID ??
        a?.PositionID ??
        a?.position?.id ??
        a?.Position?.Id;
      if (pid !== undefined && pid !== null) ids.add(pid);
    }
    return ids;
  }

  startInterview(sessionId: string): void {
    if (sessionId) {
      this.router.navigate(['/interview'], { queryParams: { sessionId } });
    } else {
      this.apiService.startInterview().subscribe({
        next: (response: any) => {
          const sid =
            response?.sessionId ||
            response?.SessionId ||
            response?.SessionID ||
            response?.sessionID;

          if (sid) {
            this.router.navigate(['/interview'], { queryParams: { sessionId: sid } });
          } else {
            this.router.navigate(['/interview']);
          }
        },
      });
    }
  }

  getTechStackList(techStack: string): string[] {
    return techStack ? techStack.split(',').map((s) => s.trim()) : [];
  }
}
