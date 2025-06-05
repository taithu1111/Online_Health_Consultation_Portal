import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '@core/service/user.service';
import { DomSanitizer } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  imports: [
      BreadcrumbComponent,
      MatFormFieldModule,
      MatInputModule,
      MatButtonModule,
      MatDatepickerModule,
      MatSelectModule,
      MatOptionModule,
      ReactiveFormsModule,
      CommonModule
  ],
  standalone: true
})
export class SettingsComponent implements OnInit {
  profileForm!: FormGroup;
  passwordForm!: FormGroup;

  selectedImageFile: File | null = null;
  imagePreviewUrl: any = null;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.initForms();
    this.loadUserProfile();
  }

  initForms() {
    this.profileForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: [{ value: '', disabled: true }],
      mobile: [''],
      dateOfBirth: [''],
      address: ['']
    });

    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  onDateChange(event: any) {
    if (!event.value) {
      this.profileForm.get('dateOfBirth')?.setValue(null);
    }
  }

  loadUserProfile() {
    this.userService.getProfile().subscribe(profile => {
      const nameParts = profile.fullName?.split(' ') ?? [''];
      this.profileForm.patchValue({
        firstName: nameParts[0],
        lastName: nameParts.slice(1).join(' '),
        email: profile.email,
        mobile: profile.phoneNumber,
        dateOfBirth: profile.dateOfBirth,
        address: profile.address
      });

      if (profile.imageUrl) {
        const fullUrl = profile.imageUrl.startsWith('http')
          ? profile.imageUrl
          : `http://localhost:5175${profile.imageUrl}`;
        this.imagePreviewUrl = fullUrl;
      }
    });
  }

  passwordMatchValidator(group: FormGroup) {
    return group.get('newPassword')!.value === group.get('confirmPassword')!.value
      ? null : { mismatch: true };
  }

  onImageSelected(event: Event): void {
    const file = (event.target as HTMLInputElement)?.files?.[0];
    if (file) {
      this.selectedImageFile = file;
      const objectUrl = URL.createObjectURL(file);
      this.imagePreviewUrl = this.sanitizer.bypassSecurityTrustUrl(objectUrl);
    }
  }

  saveProfile(): void {
    const formData = new FormData();
    const formValue = this.profileForm.getRawValue();

    const fullName = `${formValue.firstName} ${formValue.lastName}`.trim();

    formData.append('fullName', fullName);
    if (formValue.mobile) formData.append('phone', formValue.mobile);
    if (formValue.dateOfBirth instanceof Date) {
      formData.append('dateOfBirth', formValue.dateOfBirth.toISOString());
    } else if (typeof formValue.dateOfBirth === 'string') {
      const parsedDate = new Date(formValue.dateOfBirth);
      if (!isNaN(parsedDate.getTime())) {
        formData.append('dateOfBirth', parsedDate.toISOString());
      }
    }
    if (formValue.address) formData.append('address', formValue.address);

    if (this.selectedImageFile) {
      formData.append('profileImage', this.selectedImageFile);
    }

    this.userService.updateProfile(formData).subscribe({
      next: () => alert('Profile updated successfully.'),
      error: err => alert('Update failed: ' + (err.error?.details || err.message))
    });
  }

  changePassword(): void {
    const { currentPassword, newPassword } = this.passwordForm.value;

    this.userService.changePassword(currentPassword, newPassword).subscribe({
      next: () => alert('Password changed successfully.'),
      error: err => alert('Password change failed: ' + err.message)
    });
  }
}
