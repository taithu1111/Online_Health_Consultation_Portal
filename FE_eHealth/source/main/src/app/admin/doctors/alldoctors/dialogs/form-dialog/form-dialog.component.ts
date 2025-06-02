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
import { DoctorsService } from '../../doctors.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

interface DialogData {
  user?: User;
  doctor?: User;
  action: 'edit' | 'add';
}

@Component({
  selector: 'app-all-doctors-form-dialog',
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
export class AllDoctorFormDialogComponent implements OnInit {
  action: string;
  dialogTitle: string;
  doctorForm: UntypedFormGroup;
  doctor: User = {
    id: 0,
    imageUrl: 'assets/images/user/user1.jpg',
    email: '',
    fullName: '',
    role: 'Doctor',
    phoneNumber: '',
    specialization: '',
    experienceYears: 0,
    consultationFee: 0,
    languages: '',
    bio: ''
  };

  constructor(
    public dialogRef: MatDialogRef<AllDoctorFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: UntypedFormBuilder,
    private userService: UserService,
    private doctorService: DoctorsService,
    private snackBar: MatSnackBar
  ) {
    this.action = data.action;
    
    if (data.user) {
      this.doctor = { ...this.doctor, ...data.user };
    } else if (data.doctor) {
      this.doctor = { ...this.doctor, ...data.doctor };
    }

    this.dialogTitle = this.action === 'edit' 
      ? `Edit ${this.doctor.fullName || 'Doctor'}` 
      : 'New Doctor';
    
    this.doctorForm = this.createDoctorForm();
  }

  ngOnInit(): void {
    console.log(this.doctor.fullName + this.doctor.phoneNumber + this.doctor.specialization);
    this.initializeForm();
  }

  private initializeForm(): void {
    this.doctorForm = this.fb.group({
      name: [this.doctor.fullName || '', Validators.required],
      phone: [this.doctor.phoneNumber || '', [Validators.required, Validators.pattern('^[0-9]*$')]],
      email: [this.doctor.email || '', [Validators.required, Validators.email]],
      specialization: [this.doctor.specialization || '', Validators.required],
      experience: [this.doctor.experienceYears || 0, Validators.required],
      fee: [this.doctor.consultationFee || 0, Validators.required],
      languages: [this.doctor.languages || ''],
      bio: [this.doctor.bio || '']
    });
  }

  createDoctorForm(): UntypedFormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      email: ['', [Validators.email, Validators.required]],
      specialization: ['', Validators.required],
      experience: [0, Validators.required],
      fee: [0, Validators.required],
      languages: [''],
      bio: ['']
    });
  }

  submit() {
    if (!this.doctorForm.valid) return;
    console.log('SUBMIT BUTTON CLICKED');


    const formValue = this.doctorForm.value;
    const payload: UpdateUserProfileDto = {
      fullName: formValue.name,
      phone: formValue.phone,
      specialization: formValue.specialization,
      experienceYears: formValue.experience,
      consultationFee: formValue.fee,
      languages: formValue.languages,
      bio: formValue.bio
    };

    
    console.log('Action:', this.action);
    console.log('Patient ID:', this.doctor.id);
    console.log('Payload:', payload);

    if (this.action === 'edit' && this.doctor?.id) {
      this.userService.updateProfileByAdmin(this.doctor.id, payload).subscribe({
        next: () => {
          this.snackBar.open('Doctor profile updated successfully!', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            panelClass: ['snackbar-success']
          });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Admin Update Error:', error);
          this.snackBar.open('Failed to update doctor profile.', 'Close', {
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