import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '@core';
import { finalize } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatError, MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatError,
    MatButtonModule,
    RouterModule
  ]
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  isLoading = false;
  isSubmitted = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  get f() {
    return this.forgotPasswordForm.controls;
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.forgotPasswordForm.invalid) {
      return;
    }

    this.isLoading = true;
    
    this.authService.forgotPassword(this.forgotPasswordForm.value.email)
      .pipe(
        finalize(() => this.isLoading = false)
      )
      .subscribe({
        next: () => {
          this.snackBar.open('Password reset link has been sent to your email', 'Close', {
            duration: 5000,
            panelClass: 'success-snackbar'
          });
          this.isSubmitted = false;
          // this.forgotPasswordForm.reset();
        },
        error: (error) => {
          const errorMessage = error.error?.message || 'Failed to send reset link';
          this.snackBar.open(errorMessage, 'Close', {
            duration: 5000,
            panelClass: 'error-snackbar'
          });
        }
      });
  }
}