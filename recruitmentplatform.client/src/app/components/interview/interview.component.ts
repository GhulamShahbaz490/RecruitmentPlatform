// interview.component.ts
import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NgbAlertModule, NgbProgressbarModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { InterviewQuestionDto, SubmitAnswerDto } from '../../models/interview.model';
import { Subject, interval, takeUntil } from 'rxjs';

@Component({
  selector: 'app-interview',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NgbAlertModule,
    NgbProgressbarModule
  ],
  templateUrl: './interview.component.html',
  styleUrls: ['./interview.component.css']
})
export class InterviewComponent implements OnDestroy {
  sessionId: string = '';
  currentQuestion: InterviewQuestionDto | null = null;
  selectedAnswer: number | null = null;
  timeLeft: number | string = '...';
  totalQuestions = 0;
  answeredCount = 0;
  isLoading = true;
  isSubmitting = false;
  alertMessage: string = '';
  alertType: string = 'danger';

  private timerSubscription = new Subject<void>();
  private destroy$ = new Subject<void>();

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {
    this.startInterview();
  }

ngOnInit(): void {
    // Check for existing session ID
    this.router.routerState.root.queryParams.subscribe(params => {
      const sessionId = params['sessionId'];
      if (sessionId) {
        this.sessionId = sessionId;
        this.loadNextQuestion();
      } else {
        this.startInterview();
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  showAlert(message: string, type: string = 'danger'): void {
    this.alertMessage = message;
    this.alertType = type;
    setTimeout(() => this.alertMessage = '', 3000);
  }

  startInterview(): void {
    this.apiService.startInterview().subscribe({
      next: (response) => {
        this.sessionId = response.sessionId;
        this.loadNextQuestion();
      },
      error: (err) => {
        this.showAlert('Failed to start interview');
        this.router.navigate(['/']);
      }
    });
  }

  loadNextQuestion(): void {
    this.apiService.getNextQuestion(this.sessionId).subscribe({
      next: (question) => {
        if (question && question.id) {
          this.currentQuestion = question;
          this.selectedAnswer = null;
          this.timeLeft = question.timeLimitSeconds;
          this.startTimer();
          this.isLoading = false;
          this.totalQuestions++;
        } else {
          this.completeInterview();
        }
      },
      error: (err) => {
        this.completeInterview();
      }
    });
  }

  startTimer(): void {
    interval(1000).pipe(
      takeUntil(this.timerSubscription),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      if (typeof this.timeLeft === 'number') {
        this.timeLeft = this.timeLeft - 1;
        if (this.timeLeft <= 0) {
          this.timeUp();
        }
      }
    });
  }

  timeUp(): void {
    this.timerSubscription.next();
    if (this.selectedAnswer === null) {
      this.selectedAnswer = -1; // Mark as unanswered
    }
    this.submitAnswer();
  }

  onSubmitAnswer(): void {
    if (this.selectedAnswer === null) {
      this.showAlert('Please select an answer', 'warning');
      return;
    }
    this.timerSubscription.next();
    this.submitAnswer();
  }

  getProgress(): number {
    if (!this.currentQuestion || typeof this.timeLeft !== 'number') return 0;
    return (this.timeLeft / this.currentQuestion.timeLimitSeconds) * 100;
  }

  isTimeWarning(): boolean {
    return typeof this.timeLeft === 'number' && this.timeLeft < 10;
  }

  private submitAnswer(): void {
    if (!this.currentQuestion || typeof this.timeLeft !== 'number') return;

    this.isSubmitting = true;
    const timeTaken = this.currentQuestion.timeLimitSeconds - this.timeLeft;

    const answerDto: SubmitAnswerDto = {
      questionId: this.currentQuestion.id,
      selectedAnswerIndex: this.selectedAnswer!,
      timeTakenSeconds: timeTaken
    };

    this.apiService.submitAnswer(this.sessionId, answerDto).subscribe({
      next: () => {
        this.answeredCount++;
        this.isSubmitting = false;
        this.loadNextQuestion();
      },
      error: (err) => {
        this.showAlert('Failed to submit answer');
        this.loadNextQuestion();
      }
    });
  }

  completeInterview(): void {
    this.isLoading = true;
    this.apiService.completeInterview(this.sessionId).subscribe({
      next: (result) => {
        this.router.navigate(['/results'], { state: { result } });
      },
      error: (err) => {
        this.showAlert('Failed to complete interview');
        this.router.navigate(['/']);
      }
    });
  }
}
