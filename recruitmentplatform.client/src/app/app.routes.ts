import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ApplyComponent } from './components/apply/apply.component';
import { LoginComponent } from './components/login/login.component';
import { InterviewComponent } from './components/interview/interview.component';
import { ResultsComponent } from './components/results/results.component';
import { authGuard } from './guards/auth.guard';
import { ADMIN_ROUTES } from './admin.routes';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'apply/:id', component: ApplyComponent },
  { path: 'login', component: LoginComponent },
  { path: 'interview', component: InterviewComponent, canActivate: [authGuard] },
  { path: 'results', component: ResultsComponent, canActivate: [authGuard] },
  ...ADMIN_ROUTES,
  { path: '**', redirectTo: '' }
];
