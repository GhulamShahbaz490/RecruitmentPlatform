// login.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbAlertModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  alertMessage: string = '';
  alertType: string = 'danger';
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      pinCode: ['', [Validators.required, Validators.pattern('^[0-9]{6}$')]]
    });
  }

  showAlert(message: string, type: string = 'danger'): void {
    this.alertMessage = message;
    this.alertType = type;
    setTimeout(() => this.alertMessage = '', 5000);
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.showAlert('Please enter valid credentials');
      return;
    }

    const loginData = {
      email: this.loginForm.value.email || '',
      pinCode: this.loginForm.value.pinCode || ''
    };

    this.authService.login(loginData).subscribe({
      next: () => {
        this.showAlert('Login successful!', 'success');
        this.router.navigate(['/interview']);
      },
      error: (err) => {
        this.showAlert(err.error || 'Invalid credentials');
      }
    });
  }
}