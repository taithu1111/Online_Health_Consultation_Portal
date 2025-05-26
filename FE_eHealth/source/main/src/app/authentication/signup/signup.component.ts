import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService, Role } from '@core';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    RouterLink,
    MatButtonModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    CommonModule
  ]
})
export class SignupComponent implements OnInit {
  authForm!: UntypedFormGroup;
  submitted = false;
  returnUrl!: string;
  hide = true;
  chide = true;

  allCriteriaMet = false; // Password validating on front-end
  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) { }
  ngOnInit() {
    this.authForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: [
        '',
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/[A-Z]/), // at least one uppercase
          Validators.pattern(/[a-z]/), // at least one lowercase
          Validators.pattern(/[0-9]/), // at least one number
          Validators.pattern(/[\W_]/)  // at least one special character
        ]
      ],
      phoneNumber: ['', Validators.required],
      cpassword: ['', Validators.required],
      gender: ['', Validators.required],
      role: [Role.Patient, Validators.required],
      dateOfBirth: ['', Validators.required],
      address: ['', Validators.required],
      bloodType: [''] // default to '', optional field
    }, { validator: this.passwordsMatchValidator });
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
  passwordsMatchValidator(form: UntypedFormGroup) {
    const pass = form.get('password')?.value;
    const cpass = form.get('cpassword')?.value;

    if (!pass || !cpass) return null; // Skip if either is empty
    return pass === cpass ? null : { mismatch: true };
  }
  get f() {
    return this.authForm.controls;
  }
  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.authForm.invalid) {
      return;
    }

    const formValues = this.authForm.value;
    // console.log('Form Data:', formValues);

    this.authService.register({
      fullName: formValues.username,
      email: formValues.email,
      password: formValues.password,
      confirmPassword: formValues.cpassword,
      gender: formValues.gender,
      role: Role.Patient,
      createdAt: new Date().toISOString(),
      phoneNumber: formValues.phoneNumber,
      dateOfBirth: formValues.dateOfBirth,
      address: formValues.address,
      bloodType: formValues.bloodType
    }).subscribe({
      next: (response) => {
        // console.log('Registration successful:', response);
        this.router.navigate(['authentication/signin']);
      },
      error: err => {
        console.error('Error:', err);
        alert(err.message);
      }
    });
  }

  hasMinLength() {
    const pass = this.authForm.get('password')?.value || '';
    return pass.length >= 6;
  }

  hasUpperCase() {
    return /[A-Z]/.test(this.authForm.get('password')?.value || '');
  }

  hasLowerCase() {
    return /[a-z]/.test(this.authForm.get('password')?.value || '');
  }

  hasNumber() {
    return /[0-9]/.test(this.authForm.get('password')?.value || '');
  }

  hasSpecialChar() {
    return /[\W_]/.test(this.authForm.get('password')?.value || '');
  }

  ngDoCheck() {
    const passwordControl = this.authForm.get('password');
    if (passwordControl?.dirty) {
      const allValid = this.hasMinLength() && 
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