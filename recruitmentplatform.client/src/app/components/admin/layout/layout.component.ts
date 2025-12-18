// layout.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { NgbCollapseModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    NgbCollapseModule,
    NgbAlertModule
  ],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class AdminLayoutComponent {
  adminName = '';
  alertMessage = '';
  alertType = 'success';
  isCollapsed = false;

  navItems = [
    { path: '/admin/dashboard', icon: 'bi-speedometer2', label: 'Dashboard' },
    { path: '/admin/positions', icon: 'bi-briefcase', label: 'Positions' },
    { path: '/admin/questions', icon: 'bi-question-circle', label: 'Questions' },
    { path: '/admin/settings', icon: 'bi-gear', label: 'Settings' }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.adminName = this.authService.getAdminName() || 'Admin';
  }

  showAlert(message: string, type: string = 'success'): void {
    this.alertMessage = message;
    this.alertType = type;
    setTimeout(() => this.alertMessage = '', 3000);
  }

  logout(): void {
    this.authService.logoutAdmin();
    this.showAlert('Logged out successfully');
    this.router.navigate(['/admin/login']);
  }
}