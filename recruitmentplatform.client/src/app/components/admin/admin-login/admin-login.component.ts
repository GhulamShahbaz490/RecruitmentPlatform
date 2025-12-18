// admin-login.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../services/auth.service';
import { AdminLoginDto } from '../../../models/admin.model';
import Swal from 'sweetalert2';

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
      Swal.fire({ text: 'Please enter valid credentials', icon: 'warning', confirmButtonText: 'OK' });
      return;
    }

    this.authService.loginAdmin(this.loginForm.value as AdminLoginDto).subscribe({
      next: () => {
        Swal.fire({ text: 'Admin login successful!', icon: 'success', timer: 1000, showConfirmButton: false });
        setTimeout(() => this.router.navigate(['/admin/dashboard']), 1000);
      },
      error: (err) => {
        Swal.fire({ text: err.error || 'Invalid admin credentials', icon: 'error', confirmButtonText: 'OK' });
      }
    });
  }
}
