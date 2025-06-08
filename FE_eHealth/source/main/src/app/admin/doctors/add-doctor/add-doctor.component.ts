import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, UntypedFormBuilder, Validators } from '@angular/forms';
import { DoctorsService } from '../alldoctors/doctors.service';
import { SpecializationService } from '@core/service/specialization.service';
import { MatOptionModule } from '@angular/material/core';
import { FileUploadComponent } from '@shared/components/file-upload/file-upload.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-doctor',
  templateUrl: './add-doctor.component.html',
  styleUrls: ['./add-doctor.component.scss'],
  imports: [
    FormsModule,
    MatOptionModule,
    FileUploadComponent,
    MatFormFieldModule,
    ReactiveFormsModule,
    BreadcrumbComponent,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    CommonModule
  ],
  standalone: true
})
export class AddDoctorComponent implements OnInit {
  docForm!: FormGroup;
  specializations: any[] = [];
  allCriteriaMet = false;
  hide = true;

  constructor(
    private fb: UntypedFormBuilder,
    private doctorService: DoctorsService,
    private specializationService: SpecializationService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.docForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/[A-Z]/), // uppercase
          Validators.pattern(/[a-z]/), // lowercase
          Validators.pattern(/[0-9]/), // number
          Validators.pattern(/[\W_]/), // special character
        ]
      ],
      gender: ['', Validators.required],
      experienceYears: [0, [Validators.required, Validators.min(0)]],
      languages: ['', Validators.required],
      consultationFee: [0, [Validators.required, Validators.min(0)]],
      bio: ['', Validators.required],
      specializationIds: [[]],
      profileImage: [null]
    });

    this.loadSpecializations();
  }

  private loadSpecializations() {
    this.specializationService.getAllSpecializations().subscribe(specs => {
      this.specializations = specs;
    });
  }

  onSubmit() {
    if (this.docForm.invalid) return;

    const formData = new FormData();
    Object.entries(this.docForm.value).forEach(([key, value]) => {
      if (key === 'specializationIds') {
        (value as number[]).forEach(id => formData.append('SpecializationIds', id.toString()));
      } else if (key === 'profileImage' && value instanceof File) {
        formData.append('ProfileImage', value);
      } else {
        formData.append(key, value as string);
      }
    });

    this.doctorService.addDoctor(formData).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(response.message, 'Close', { duration: 3000 });
          this.docForm.reset();
        } else {
          this.snackBar.open(response.message, 'Close', { duration: 3000, panelClass: ['error-snackbar'] });
        }
      },
      error: (err) => {
        this.snackBar.open(err.message || 'An unexpected error occurred', 'Close', { 
          duration: 5000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }

  onFileChange(event: any) {
    const file = event.target.files?.[0];
    this.docForm.patchValue({ profileImage: file });
  }

  hasMinLength() {
    const pass = this.docForm.get('password')?.value || '';
    return pass.length >= 6;
  }
  hasUpperCase() {
    return /[A-Z]/.test(this.docForm.get('password')?.value || '');
  }
  hasLowerCase() {
    return /[a-z]/.test(this.docForm.get('password')?.value || '');
  }
  hasNumber() {
    return /[0-9]/.test(this.docForm.get('password')?.value || '');
  }
  hasSpecialChar() {
    return /[\W_]/.test(this.docForm.get('password')?.value || '');
  }

  ngDoCheck() {
    const passwordControl = this.docForm.get('password');
    if (passwordControl?.dirty) {
      const allValid =
        this.hasMinLength() &&
        this.hasUpperCase() &&
        this.hasLowerCase() &&
        this.hasNumber() &&
        this.hasSpecialChar();

      if (allValid && !this.allCriteriaMet) {
        this.allCriteriaMet = true;
        setTimeout(() => {
          this.allCriteriaMet = false;
        }, 1500);
      }
    }
  }
}
