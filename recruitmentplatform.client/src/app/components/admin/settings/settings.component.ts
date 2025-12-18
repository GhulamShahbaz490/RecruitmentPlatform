// settings.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AdminApiService } from '../../../services/admin-api.service';
import { SettingsDto } from '../../../models/admin.model';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbAlertModule
  ],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  settingsForm: FormGroup;
  alertMessage = '';
  alertType: 'success' | 'danger' = 'success';
  showAlert = false;

  constructor(
    private fb: FormBuilder,
    private adminApiService: AdminApiService
  ) {
    this.settingsForm = this.fb.group({
      passThresholdPercentage: [70, [Validators.required, Validators.min(50), Validators.max(100)]],
      questionTimeLimitSeconds: [60, [Validators.required, Validators.min(30), Validators.max(300)]],
      totalQuestionsPerSection: [5, [Validators.required, Validators.min(3), Validators.max(20)]],
      randomizeQuestions: [true],
      emailNotificationsEnabled: [true]
    });
  }

  ngOnInit(): void {
    this.loadSettings();
  }

  loadSettings(): void {
    this.adminApiService.getSettings().subscribe({
      next: (settings) => {
        this.settingsForm.patchValue(settings);
      },
      error: () => {
        this.showAlertMessage('Failed to load settings', 'danger');
      }
    });
  }

  saveSettings(): void {
    if (this.settingsForm.invalid) {
      this.showAlertMessage('Please check all settings', 'danger');
      return;
    }

    this.adminApiService.updateSettings(this.settingsForm.value as SettingsDto).subscribe({
      next: () => {
        this.showAlertMessage('Settings saved successfully', 'success');
      },
      error: () => {
        this.showAlertMessage('Failed to save settings', 'danger');
      }
    });
  }

  resetToDefaults(): void {
    this.settingsForm.reset({
      passThresholdPercentage: 70,
      questionTimeLimitSeconds: 60,
      totalQuestionsPerSection: 5,
      randomizeQuestions: true,
      emailNotificationsEnabled: true
    });
  }

  showAlertMessage(message: string, type: 'success' | 'danger'): void {
    this.alertMessage = message;
    this.alertType = type;
    this.showAlert = true;
    setTimeout(() => this.showAlert = false, 3000);
  }
}