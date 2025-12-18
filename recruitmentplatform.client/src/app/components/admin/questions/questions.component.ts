// question-management.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AdminApiService } from '../../../services/admin-api.service';
import { QuestionManagementDto } from '../../../models/admin.model';

@Component({
  selector: 'app-question-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbAlertModule
  ],
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionManagementComponent implements OnInit {
  questions: QuestionManagementDto[] = [];
  alertMessage = '';
  alertType = 'danger';
  questionForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private adminApiService: AdminApiService
  ) {
    this.questionForm = this.fb.group({
      section: ['Basic', Validators.required],
      text: ['', Validators.required],
      options: this.fb.array(['', '', '', '']),
      correctAnswerIndex: [0, Validators.required],
      points: [10, [Validators.required, Validators.min(5)]],
      explanation: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadQuestions();
  }

  get optionsArray(): FormArray {
    return this.questionForm.get('options') as FormArray;
  }

  showAlert(message: string, type: string = 'danger'): void {
    this.alertMessage = message;
    this.alertType = type;
    setTimeout(() => this.alertMessage = '', 5000);
  }

  loadQuestions(): void {
    this.adminApiService.getAllQuestions().subscribe({
      next: (questions) => {
        this.questions = questions;
      },
      error: () => {
        this.showAlert('Failed to load questions');
      }
    });
  }

  createQuestion(): void {
    if (this.questionForm.invalid) {
      this.showAlert('Please fill all required fields');
      return;
    }

    const question = this.questionForm.value as QuestionManagementDto;
    this.adminApiService.createQuestion(question).subscribe({
      next: () => {
        this.showAlert('Question created successfully', 'success');
        this.loadQuestions();
        this.questionForm.reset({ section: 'Basic', correctAnswerIndex: 0, points: 10 });
      },
      error: () => {
        this.showAlert('Failed to create question');
      }
    });
  }

  deleteQuestion(id: string): void {
    if (confirm('Are you sure you want to delete this question?')) {
      this.adminApiService.deleteQuestion(id).subscribe({
        next: () => {
          this.showAlert('Question deleted', 'success');
          this.loadQuestions();
        },
        error: () => {
          this.showAlert('Failed to delete question');
        }
      });
    }
  }
}