import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '@core/service/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '@core';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss'],
    imports: [
        BreadcrumbComponent,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        CommonModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        MatIconModule
    ]
})
export class SettingsComponent implements OnInit {
  passwordForm!: FormGroup;
  profileForm!: FormGroup;
  imageFile: File | null = null;
  imagePreviewUrl: string | null = null;

  hideCurrentPassword: boolean = true;
  hideNewPassword: boolean = true;
  hideConfirmPassword: boolean = true;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.initForms();
    this.loadProfile();
  }

  private initForms(): void {
    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });

    this.profileForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: [{ value: '', disabled: true }],
      mobile: [''],
      address: [''],
      bio: [''],
      languages: [''],
      experienceYears: [''],
      consultationFee: ['']
    });
  }

  private loadProfile(): void {
    this.userService.getProfile().subscribe({
      next: (user) => {
        const nameParts = user.fullName?.split(' ') ?? [''];
        this.profileForm.patchValue({
          firstName: nameParts[0],
          lastName: nameParts.slice(1).join(' '),
          email: user.email || '',
          phone: user.phoneNumber || '',
          address: user.address || '',
          bio: user.bio || '',
          languages: user.languages || [],
          experienceYears: user.experienceYears || '',
          consultationFee: user.consultationFee || ''
        });

        if (user.imageUrl) {
          this.imagePreviewUrl = user.imageUrl;
        }
      },
      error: () => {
        this.snackBar.open('Failed to load profile.', 'Close', { duration: 3000 });
      }
    });
  }

  saveProfile(): void {
    if (this.profileForm.invalid) return;

    const formData = new FormData();
    const formValues = this.profileForm.getRawValue();

    // Append all form values
    formData.append('firstName', formValues.firstName);
    formData.append('lastName', formValues.lastName);
    formData.append('mobile', formValues.mobile);
    formData.append('address', formValues.address);
    formData.append('bio', formValues.bio);
    formData.append('languages', formValues.languages);
    formData.append('experienceYears', formValues.experienceYears);
    formData.append('consultationFee', formValues.consultationFee);

    // Append the image file if it exists
    if (this.imageFile) {
      formData.append('profileImage', this.imageFile);
    }

    this.userService.updateProfile(formData).subscribe({
      next: () => {
        this.snackBar.open('Profile updated successfully.', 'Close', { duration: 3000 });
      },
      error: (error) => {
        console.error('Update error:', error);
        this.snackBar.open('Failed to update profile.', 'Close', { duration: 3000 });
      }
    });
  }

  onChangePassword(): void {
    if (this.passwordForm.invalid) return;

    const { currentPassword, newPassword, confirmPassword } = this.passwordForm.value;

    if (newPassword !== confirmPassword) {
      this.passwordForm.get('confirmPassword')?.setErrors({ mismatch: true });
      return;
    }

    this.authService.changePassword(currentPassword, newPassword, confirmPassword).subscribe({
      next: () => {
        this.snackBar.open('Password changed successfully.', 'Close', { duration: 3000 });
        this.passwordForm.reset();
      },
      error: () => {
        this.snackBar.open('Password change failed.', 'Close', { duration: 3000 });
      }
    });
  }

  onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    this.imageFile = input.files[0];

    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreviewUrl = reader.result as string;
    };
    reader.readAsDataURL(this.imageFile);
  }
}
