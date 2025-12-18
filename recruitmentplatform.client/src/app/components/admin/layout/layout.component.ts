// layout.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    NgbCollapseModule
  ],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class AdminLayoutComponent {
  adminName = '';
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

  logout(): void {
    this.authService.logoutAdmin();
    Swal.fire({ text: 'Logged out successfully', icon: 'success', timer: 1500, showConfirmButton: false });
    this.router.navigate(['/admin/login']);
  }
}
