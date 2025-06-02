import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject, OnInit } from '@angular/core';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule, formatDate } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { UserService, UpdateUserProfileDto, User } from '@core/service/user.service';
import { PatientService } from '../../patient.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

interface DialogData {
  user?: User;
  patient?: User;
  action: 'edit' | 'add';
}

@Component({
  selector: 'app-all-patients-form-dialog',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatDialogContent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatDatepickerModule,
    MatDialogClose,
    MatSnackBarModule
  ]
})
export class AllPatientFormDialogComponent implements OnInit {
  action: string;
  dialogTitle: string;
  patientForm: UntypedFormGroup;
  patient: User = {
    id: 0,
    imageUrl: 'assets/images/user/user1.jpg',
    email: '',
    fullName: '',
    gender: 'male',
    role: 'Patient',
    phoneNumber: '',
    address: '',
    bloodType: '',
    dateOfBirth: ''
  };

  constructor(
    public dialogRef: MatDialogRef<AllPatientFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: UntypedFormBuilder,
    private userService: UserService,
    private patientService: PatientService,
    private snackBar: MatSnackBar
  ) {
    // Initialize with safe defaults
    this.action = data.action;
    
    // Merge incoming data with defaults
    if (data.user) {
      this.patient = { ...this.patient, ...data.user };
    } else if (data.patient) {
      this.patient = { ...this.patient, ...data.patient };
    }

    this.dialogTitle = this.action === 'edit' 
      ? `Edit ${this.patient.fullName || 'Patient'}` 
      : 'New Patient';
    
    this.patientForm = this.createPatientForm();
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    const nameParts = this.patient.fullName?.split(' ') || ['', ''];
    this.patientForm.patchValue({
      first: nameParts[0] || '',
      last: nameParts.length > 1 ? nameParts.slice(1).join(' ') : '',
      gender: this.patient.gender || 'male',
      mobile: this.patient.phoneNumber || '',
      email: this.patient.email || '',
      bType: this.patient.bloodType || '',
      address: this.patient.address || '',
      dob: this.patient.dateOfBirth ? new Date(this.patient.dateOfBirth) : null
    });
  }

  createPatientForm(): UntypedFormGroup {
    return this.fb.group({
      first: ['', Validators.required],
      last: [''],
      gender: ['male', Validators.required],
      mobile: ['', [Validators.pattern('^[0-9]*$')]],
      email: ['', [Validators.email, Validators.required]],
      bType: [''],
      address: [''],
      dob: ['']
    });
  }

  submit() {
    if (!this.patientForm.valid) return;

    const formValue = this.patientForm.value;
    const payload: UpdateUserProfileDto = {
      fullName: `${formValue.first} ${formValue.last}`.trim(),
      gender: formValue.gender,
      phone: formValue.mobile,
      dateOfBirth: formValue.dob ? formatDate(formValue.dob, 'yyyy-MM-dd', 'en') : undefined,
      bloodType: formValue.bType,
      address: formValue.address
    };

    if (this.action === 'edit' && this.patient?.id) {
      this.userService.updateProfileByAdmin(this.patient.id, payload).subscribe({
        next: () => {
          this.snackBar.open('Patient profile updated successfully!', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            panelClass: ['snackbar-success'] // You can customize the style
          });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Admin Update Error:', error);
          this.snackBar.open('Failed to update patient profile.', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            panelClass: ['snackbar-error']
          });
        }
      });
    } else {
      console.warn('Unhandled form action:', this.action);
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}