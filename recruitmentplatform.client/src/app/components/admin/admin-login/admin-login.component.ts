// admin-login.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../services/auth.service';
import { AdminLoginDto } from '../../../models/admin.model';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbAlertModule
  ],
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.css']
})
export class AdminLoginComponent {
  loginForm: FormGroup;
  alertMessage = '';
  alertType: 'success' | 'danger' = 'success';
  showAlert = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {
    this.loginForm = this.fb.group({
      email: ['admin@recruit.com', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.showAlertMessage('Please enter valid credentials', 'danger');
      return;
    }

    this.authService.loginAdmin(this.loginForm.value as AdminLoginDto).subscribe({
      next: () => {
        this.showAlertMessage('Admin login successful!', 'success');
        setTimeout(() => this.router.navigate(['/admin/dashboard']), 1000);
      },
      error: (err) => {
        this.showAlertMessage(err.error || 'Invalid admin credentials', 'danger');
      }
    });
  }

  showAlertMessage(message: string, type: 'success' | 'danger'): void {
    this.alertMessage = message;
    this.alertType = type;
    this.showAlert = true;
    setTimeout(() => this.showAlert = false, 3000);
  }
}