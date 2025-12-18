// apply.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';

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
    this.alertMessage = message;
    this.alertType = type;
    setTimeout(() => this.alertMessage = '', 5000);
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file && file.type === 'application/pdf') {
      this.selectedFile = file;
      this.applicationForm.patchValue({ resume: file });
    } else {
      this.selectedFile = null;
      this.showAlert('Please select a PDF file');
    }
  }

  onSubmit(): void {
    if (this.applicationForm.invalid) {
      this.showAlert('Please fill all required fields');
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
        this.showAlert('Application submitted! Check your email for credentials.', 'success');
        this.authService.login({ 
          email: this.applicationForm.value.email || '', 
          pinCode: response.interviewNumber.split('-')[2] // Extract PIN from number
        }).subscribe({
          next: () => {
            this.router.navigate(['/interview']);
          },
          error: () => {
            this.router.navigate(['/login']);
          }
        });
      },
      error: (err) => {
        this.showAlert(err.error?.message || 'Failed to submit application');
        this.submitting = false;
      }
    });
  }
}