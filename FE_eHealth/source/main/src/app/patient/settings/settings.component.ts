import { Component } from '@angular/core';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '@core';
import { UserService, User, UpdateUserProfileDto } from '@core/service/user.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss'],
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
export class SettingsComponent {
  profileForm!: FormGroup;
  passwordForm!: FormGroup;

  constructor(private fb: FormBuilder, private userService: UserService) {}

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      mobile: [''],
      address: [''],
      city: [''],
      country: [''],
      dateOfBirth: [null],
      bloodGroup: ['']
    });

    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });

    this.loadUserProfile();
  }

  onDateChange(event: any) {
    if (!event.value) {
      this.profileForm.get('dateOfBirth')?.setValue(null);
    }
  }

  loadUserProfile(): void {
    this.userService.getProfile().subscribe(profile => {
      const [firstName, lastName] = profile.fullName?.split(' ') || ['', ''];
      
      this.profileForm.patchValue({
        firstName: firstName,
        lastName: lastName,
        email: profile.email,
        mobile: profile.phone,
        address: profile.address,
        dateOfBirth: profile.dateOfBirth || null
      });
    });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('newPassword')!.value === form.get('confirmPassword')!.value
      ? null
      : { mismatch: true };
  }

  saveProfile(): void {
    if (this.profileForm.invalid) return;

    // const dateOfBirth = this.profileForm.value.dateOfBirth || null;

    const profileData: UpdateUserProfileDto = {
      fullName: `${this.profileForm.value.firstName} ${this.profileForm.value.lastName}`,
      phone: this.profileForm.value.mobile,
      address: this.profileForm.value.address,
      dateOfBirth: this.profileForm.value.dateOfBirth?.toISOString() || null
    };

    this.userService.updateProfile(profileData).subscribe({
      next: () => alert('Profile updated successfully'),
      error: err => alert('Error updating profile: ' + err.message)
    });
  }

  changePassword(): void {
    if (this.passwordForm.invalid) return;

    const { currentPassword, newPassword } = this.passwordForm.value;

    this.userService.changePassword(currentPassword, newPassword).subscribe({
      next: () => {
        alert('Password changed successfully');
        this.passwordForm.reset();
      },
      error: err => alert('Error changing password: ' + err.message)
    });
  }
}