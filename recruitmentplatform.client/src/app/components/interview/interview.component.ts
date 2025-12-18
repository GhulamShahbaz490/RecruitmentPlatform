import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NgbAlertModule, NgbProgressbarModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-interview',
  standalone: true,
  imports: [CommonModule, FormsModule, NgbAlertModule, NgbProgressbarModule],
  templateUrl: './interview.component.html',
  styleUrls: ['./interview.component.css']
})
export class InterviewComponent implements OnInit, OnDestroy {
  sessionId: string = '';
  currentQuestion: any = null;
  selectedAnswer: number | null = null;
  timeLeft = 60;
  totalQuestions = 0;
  answeredCount = 0;
  isLoading = true;
  isSubmitting = false;
  errorMessage: string | null = null;

  private timerSubscription = new Subject<void>();
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Check if user is authenticated
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    // Check for existing session ID in query params
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(params => {
      const existingSessionId = params['sessionId'];
      if (existingSessionId) {
        this.sessionId = existingSessionId;
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

  startInterview(): void {
    this.isLoading = true;
    this.apiService.startInterview().subscribe({
      next: (response) => {
        this.sessionId = response.sessionId;
        this.loadNextQuestion();
      },
      error: (err) => {
        Swal.fire('Error', 'Failed to start interview. Please try again.', 'error');
        this.isLoading = false;
        setTimeout(() => this.router.navigate(['/']), 3000);
      }
    });
  }

  loadNextQuestion(): void {
    if (!this.sessionId) {
      this.errorMessage = 'Invalid interview session.';
      this.router.navigate(['/']);
      return;
    }

    this.apiService.getNextQuestion(this.sessionId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (question) => {
        if (!question) {
          // No more questions - complete interview
          this.completeInterview();
          return;
        }
        
        this.currentQuestion = question;
        this.selectedAnswer = null;
        this.timeLeft = question.timeLimitSeconds || 60;
        this.isLoading = false;
        this.totalQuestions++;
        this.startTimer();
      },
      error: (err) => {
        Swal.fire('Error', 'Failed to load next question.', 'error');
        this.errorMessage = 'Failed to load next question.';
        setTimeout(() => this.completeInterview(), 2000);
      }
    });
  }

  startTimer(): void {
    const interval = setInterval(() => {
      this.timeLeft--;
      if (this.timeLeft <= 0) {
        clearInterval(interval);
        this.timeUp();
      }
    }, 1000);

    this.destroy$.subscribe(() => clearInterval(interval));
  }

  timeUp(): void {
    if (this.selectedAnswer === null) {
      this.selectedAnswer = -1; // Mark as unanswered
    }
    this.submitAnswer();
  }

  onSubmitAnswer(): void {
    if (this.selectedAnswer === null) {
      Swal.fire('Info', 'Please select an answer before submitting.', 'info');
      return;
    }
    this.submitAnswer();
  }

  private submitAnswer(): void {
    if (!this.currentQuestion) return;

    this.isSubmitting = true;
    const timeTaken = 60 - this.timeLeft; // Assuming 60 seconds total

    const answerDto = {
      questionId: this.currentQuestion.id,
      selectedAnswerIndex: this.selectedAnswer!,
      timeTakenSeconds: timeTaken
    };

    this.apiService.submitAnswer(this.sessionId, answerDto).pipe(takeUntil(this.destroy$)).subscribe({
      next: (response) => {
        this.answeredCount++;
        this.isSubmitting = false;
        this.loadNextQuestion();
      },
      error: (err) => {
        Swal.fire('Error', 'Failed to submit answer. Moving to next question.', 'error');
        this.errorMessage = 'Failed to submit answer. Moving to next question.';
        this.isSubmitting = false;
        setTimeout(() => {
          this.errorMessage = null;
          this.loadNextQuestion();
        }, 2000);
      }
    });
  }

  completeInterview(): void {
    this.isLoading = true;
    this.apiService.completeInterview(this.sessionId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (result) => {
        // CRITICAL FIX: Properly navigate to results page
        this.router.navigate(['/results'], { 
          queryParams: { 
            sessionId: this.sessionId,
            completed: 'true' 
          },
          state: { result: result } // Pass result via state
        });
      },
      error: (err) => {
        Swal.fire('Error', 'Failed to complete interview. Please try again.', 'error');
        this.errorMessage = 'Failed to complete interview. Please try again.';
        this.isLoading = false;
        // Fallback navigation
        setTimeout(() => this.router.navigate(['/']), 3000);
      }
    });
  }

  getProgress(): number {
    return this.totalQuestions > 0 ? (this.answeredCount / this.totalQuestions) * 100 : 0;
  }
}
