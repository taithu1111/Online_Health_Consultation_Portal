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
    MatDatepickerModule
  ]
})
export class SignupComponent implements OnInit {
  authForm!: UntypedFormGroup;
  submitted = false;
  returnUrl!: string;
  hide = true;
  chide = true;
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
      password: ['', Validators.required],
      cpassword: ['', Validators.required],
      gender: ['', Validators.required],
      role: [Role.Patient, Validators.required],
      dateOfBirth: ['', Validators.required],
      address: ['', Validators.required]
    });
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
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
      dateOfBirth: formValues.dateOfBirth,
      address: formValues.address
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
}