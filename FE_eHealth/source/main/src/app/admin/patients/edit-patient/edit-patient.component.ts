import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { formatDate } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { FileUploadComponent } from '@shared/components/file-upload/file-upload.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { User, UserService } from '@core/service/user.service';

@Component({
  selector: 'app-edit-patient',
  templateUrl: './edit-patient.component.html',
  styleUrls: ['./edit-patient.component.scss'],
  imports: [
    BreadcrumbComponent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    FileUploadComponent,
    MatButtonModule,
  ]
})
export class EditPatientComponent implements OnInit {
  patientForm: UntypedFormGroup;
  patient!: User;

  constructor(
    private fb: UntypedFormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private userService: UserService
  ) {
    this.patientForm = this.createContactForm();
  }

  ngOnInit(): void {
    this.userService.getProfile().subscribe({
      next: (patient) => {
        this.patient = patient;
        const nameParts = patient.fullName?.split(' ') || [];
        this.patientForm.patchValue({
          first: nameParts[0] || '',
          last: nameParts.slice(1).join(' ') || '',
          gender: patient.gender,
          mobile: patient.phoneNumber,
          email: patient.email,
          bGroup: patient.bloodType,
          address: patient.address,
          dob: patient.dateOfBirth
        });
      },
      error: () => {
        this.showError('Failed to load profile');
        this.router.navigate(['/admin/patients/all-patients']);
      }
    });
  }

  onSubmit() {
    if (!this.patientForm.valid || this.patientForm.pristine) {
      this.showError('Please fill at least one field correctly');
      return;
    }

    const formValue = this.patientForm.value;
    
    console.log('Form value before sending:', this.patientForm.value);

    this.userService.updateProfile({
      fullName: `${formValue.first} ${formValue.last}`.trim(),
      gender: formValue.gender,
      phone: formValue.mobile,
      dateOfBirth: formValue.dob,
      bloodType: formValue.bGroup,
      address: formValue.address,
    }).subscribe({
      next: () => {
        this.snackBar.open('Profile updated successfully', 'Close', { duration: 3000 });
        this.router.navigate(['/admin/patients/all-patients']);
      },
      error: () => {
        this.showError('Failed to update profile');
      }
    });
  }

  private showError(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: ['error-snackbar']
    });
  }

  atLeastOneFieldFilledValidator() {
    return (formGroup: UntypedFormGroup) => {
      const controls = formGroup.controls;
      const isAtLeastOneFilled = Object.keys(controls).some(key => {
        return controls[key].value !== null && controls[key].value !== '';
      });

      return isAtLeastOneFilled ? null : { atLeastOneRequired: true };
    };
  }

  private createContactForm(): UntypedFormGroup {
    return this.fb.group({
      first: [''],
      last: [''],
      gender: [''],
      mobile: ['', [Validators.pattern('^[0-9]*$')]],
      email: ['', [Validators.email]],
      bGroup: [''],
      address: [''],
      dob: [''],
    }, {
      validator: this.atLeastOneFieldFilledValidator()
    });
  }
}
