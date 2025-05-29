import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '@core';
import { finalize } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatError } from '@angular/material/input';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule,
    MatError
  ]
})
export class ResetPasswordComponent {
  resetForm: FormGroup;
  isLoading = false;
  token: string = '';
  email: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.resetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });

    this.route.queryParams.subscribe(params => {
      this.token = params['token'] || '';
      this.email = params['email'] || ''; // Get email from URL
      
      if (this.email) {
        this.resetForm.patchValue({ email: this.email });
        this.resetForm.get('email')?.disable();
      }
    });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('newPassword')?.value === form.get('confirmPassword')?.value 
      ? null 
      : { mismatch: true };
  }

  get f() {
    return this.resetForm.controls;
  }

  onSubmit() {
    if (this.resetForm.invalid || !this.token) {
      return;
    }

    this.isLoading = true;
    const { newPassword } = this.resetForm.value;

    this.authService.resetPassword(this.email, this.token, newPassword)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: () => {
          this.snackBar.open('Password reset successfully!', 'Close', {
            duration: 5000,
            panelClass: 'success-snackbar'
          });
          this.router.navigate(['/authentication/signin']);
        },
        error: (error) => {
          this.snackBar.open(error.message || 'Password reset failed', 'Close', {
            duration: 5000,
            panelClass: 'error-snackbar'
          });
        }
      });
  }
}