// apply.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-apply',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbAlertModule
  ],
  templateUrl: './apply.component.html',
  styleUrls: ['./apply.component.css']
})
export class ApplyComponent {
  positionId: string = '';
  loading = false;
  submitting = false;
  alertMessage: string = '';
  alertType: string = 'danger';
  applicationForm: ReturnType<FormBuilder['group']>;
  selectedFile: File | null = null;
  emailSent = false;
  applicationData: any = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthService
  ) {
    this.positionId = this.route.snapshot.paramMap.get('id') || '';
    this.applicationForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      resume: [null, Validators.required]
    });
  }

  showAlert(message: string, type: string = 'danger'): void {
    const icon = type === 'success' ? 'success' : (type === 'warning' ? 'warning' : 'error');
    Swal.fire({ text: message, icon, confirmButtonText: 'OK' });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file && file.type === 'application/pdf') {
      this.selectedFile = file;
      this.applicationForm.patchValue({ resume: file });
    } else {
      this.selectedFile = null;
      this.showAlert('Please select a PDF file', 'warning');
    }
  }

  onSubmit(): void {
    if (this.applicationForm.invalid) {
      this.showAlert('Please fill all required fields', 'warning');
      return;
    }

    this.submitting = true;
    const formData = new FormData();
    formData.append('positionId', this.positionId);
    formData.append('firstName', this.applicationForm.value.firstName || '');
    formData.append('lastName', this.applicationForm.value.lastName || '');
    formData.append('email', this.applicationForm.value.email || '');
    formData.append('phone', this.applicationForm.value.phone || '');
    formData.append('resume', this.applicationForm.value.resume || '');

    this.apiService.apply(formData).subscribe({
      next: (response) => {
        this.emailSent = true;
        this.applicationData = {
          email: this.applicationForm.value.email,
          interviewNumber: response.interviewNumber,
          firstName: this.applicationForm.value.firstName
        };
        // Don't auto-login, let user view credentials first
        this.submitting = false;
      },
      error: (err) => {
        this.showAlert(err.error?.message || 'Failed to submit application', 'error');
        this.submitting = false;
      }
    });
  }

  proceedToInterview(): void {
    this.authService.login({ 
      email: this.applicationData.email || '', 
      pinCode: this.applicationData.interviewNumber.split('-')[2] // Extract PIN from number
    }).subscribe({
      next: () => {
        this.router.navigate(['/interview']);
      },
      error: () => {
        this.router.navigate(['/login']);
      }
    });
  }

  copyToClipboard(text: string): void {
    navigator.clipboard.writeText(text).then(() => {
      this.showAlert('Interview number copied to clipboard!', 'success');
    });
  }

  goBack(): void {
    this.router.navigate(['/home']);
  }
}
