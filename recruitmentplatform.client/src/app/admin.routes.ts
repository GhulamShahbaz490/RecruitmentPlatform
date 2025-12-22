import { Routes } from '@angular/router';
import { adminGuard } from './guards/admin.guard';
import { AdminLayoutComponent } from './components/admin/layout/layout.component';
import { AdminLoginComponent } from './components/admin/admin-login/admin-login.component';
import { PositionManagementComponent } from './components/admin/positions/positions.component';
import { QuestionManagementComponent } from './components/admin/questions/questions.component';
import { SettingsComponent } from './components/admin/settings/settings.component';
import { AdminDashboardComponent } from './components/admin/dashboard/dashboard.component';

export const ADMIN_ROUTES: Routes = [
  { path: 'admin/login', component: AdminLoginComponent },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [adminGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'positions', component: PositionManagementComponent },
      { path: 'candidates', loadComponent: () => import('./components/admin/candidates/candidates.component').then(m => m.CandidatesComponent) },
      { path: 'questions', component: QuestionManagementComponent },
      { path: 'settings', component: SettingsComponent },
    ],
  },
];
