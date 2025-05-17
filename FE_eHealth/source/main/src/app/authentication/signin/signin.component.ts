import { Component, OnInit, NgModule } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UnsubscribeOnDestroyAdapter } from '@shared';
import { AuthService, Role } from '@core';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { CookieService } from 'ngx-cookie-service';
@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss'],
  imports: [
    RouterLink,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    CommonModule
  ]
})
export class SigninComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit {

  Role = Role;
  authForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  error = '';
  hide = true;

  selectedRole: Role = Role.Patient; // Default

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private cookieService: CookieService
  ) {
    super();

    this.authForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  ngOnInit() {
    if (this.authService.isLoggedIn()) {
      this.redirectBasedOnRole();
      return;
    }

    const rememberedUsername = this.cookieService.get('rememberedUsername');
    const rememberedPassword = this.cookieService.get('rememberedPassword');

    this.authForm.patchValue({
      username: rememberedUsername || '',
      password: rememberedPassword || '',
      rememberMe: rememberedUsername !== ''
    });
  }

  private redirectBasedOnRole() {
    const user = this.authService.currentUserValue;
    if (!user) return;

    const returnUrl = this.authService.redirectUrl || this.getDefaultRoute(user.roles);
    this.router.navigateByUrl(returnUrl)

    // const roles: string[] = user.roles || [];
    // let targetRoute = '/authentication/signin';

    // if (roles.includes(Role.Admin)) {
    //   targetRoute = '/admin/dashboard/main';
    // } else if (roles.includes(Role.Doctor)) {
    //   targetRoute = '/doctor/dashboard';
    // } else if (roles.includes(Role.Patient)) {
    //   targetRoute = '/patient/dashboard';
    // }

    // this.router.navigate([targetRoute]);
  }

  get f() {
    return this.authForm.controls;
  }
  setRole(role: Role) {
    this.selectedRole = role;
  }
  getDefaultRoute(roles?: string[]): string {
    const role = roles?.[0] || this.selectedRole;
    switch (role) {
      case Role.Admin: return 'admin/dashboard/main';
      case Role.Doctor: return 'doctor/dashboard';
      case Role.Patient: return 'patient/dashboard';
      default: return '/authentication/signin';
    }
  }
  onSubmit() {
    this.submitted = true;
    this.loading = true;
    this.error = '';

    if (this.authForm.invalid) {
      this.error = 'Username or Password invalid!';
      this.loading = false;
      return;
    }

    const { username, password, rememberMe } = this.authForm.value;

    if (rememberMe) {
      this.cookieService.set('rememberedUsername', username, 30);
      this.cookieService.set('rememberedPassword', password, 30);
    } else {
      this.cookieService.delete('rememberedUsername');
      this.cookieService.delete('rememberedPassword');
    }

    this.subs.sink = this.authService
      .login(username, password, rememberMe)
      .subscribe({
        next: (res) => {
          if (res) {
            const roles: string[] = res.roles || [];
            const hasSelectedRole = roles.includes(this.selectedRole);

            if (!hasSelectedRole) {
              this.error = `You don't have ${this.selectedRole} priviledges.`;
              this.loading = false;
              return;
            }

            // alert(`Login success as ${this.selectedRole}`);
            setTimeout(() => {
              const returnUrl = this.authService.redirectUrl || this.getDefaultRoute();
              this.router.navigateByUrl(returnUrl);
              this.loading = false;
              // switch (this.selectedRole) {
              //   case Role.Admin:
              //     this.router.navigate(['/admin/dashboard/main']);
              //     break;
              //   case Role.Doctor:
              //     this.router.navigate(['/doctor/dashboard']);
              //     break;
              //   case Role.Patient:
              //     this.router.navigate(['/patient/dashboard']);
              //     break;
              //   default:
              //     this.router.navigate(['/authentication/signin']);
              //     break;
              // }
              // this.loading = false;
            }, 1000);
          }
        },
        error: (error) => {
          this.error = error.message || 'Login failed. Please try again.';
          // this.submitted = false;
          this.loading = false;
        },
      });
  }
}



// import { Component, OnInit } from '@angular/core';
// import { Router, ActivatedRoute, RouterLink } from '@angular/router';
// import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { UnsubscribeOnDestroyAdapter } from '@shared';
// import { AuthService, Role } from '@core';
// import { MatIconModule } from '@angular/material/icon';
// import { MatInputModule } from '@angular/material/input';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { MatButtonModule } from '@angular/material/button';
// @Component({
//   selector: 'app-signin',
//   templateUrl: './signin.component.html',
//   styleUrls: ['./signin.component.scss'],
//   imports: [
//     RouterLink,
//     MatButtonModule,
//     FormsModule,
//     ReactiveFormsModule,
//     MatFormFieldModule,
//     MatInputModule,
//     MatIconModule,
//   ]
// })
// export class SigninComponent
//   extends UnsubscribeOnDestroyAdapter
//   implements OnInit {
//   authForm!: UntypedFormGroup;
//   submitted = false;
//   loading = false;
//   error = '';
//   hide = true;
//   constructor(
//     private formBuilder: UntypedFormBuilder,
//     private route: ActivatedRoute,
//     private router: Router,
//     private authService: AuthService
//   ) {
//     super();
//   }

//   ngOnInit() {
//     this.authForm = this.formBuilder.group({
//       username: ['admin@hospital.org', Validators.required],
//       password: ['admin@123', Validators.required],
//     });
//   }
//   get f() {
//     return this.authForm.controls;
//   }
//   adminSet() {
//     this.authForm.get('username')?.setValue('admin@hospital.org');
//     this.authForm.get('password')?.setValue('admin@123');
//   }
//   doctorSet() {
//     this.authForm.get('username')?.setValue('doctor@hospital.org');
//     this.authForm.get('password')?.setValue('doctor@123');
//   }
//   patientSet() {
//     this.authForm.get('username')?.setValue('patient@hospital.org');
//     this.authForm.get('password')?.setValue('patient@123');
//   }
//   onSubmit() {
//     this.submitted = true;
//     this.loading = true;
//     this.error = '';
//     if (this.authForm.invalid) {
//       this.error = 'Username and Password not valid !';
//       return;
//     } else {
//       this.subs.sink = this.authService
//         .login(this.f['username'].value, this.f['password'].value)
//         .subscribe({
//           next: (res) => {
//             if (res) {
//               setTimeout(() => {
//                 const role = this.authService.currentUserValue.role;
//                 if (role === Role.All || role === Role.Admin) {
//                   this.router.navigate(['/admin/dashboard/main']);
//                 } else if (role === Role.Doctor) {
//                   this.router.navigate(['/doctor/dashboard']);
//                 } else if (role === Role.Patient) {
//                   this.router.navigate(['/patient/dashboard']);
//                 } else {
//                   this.router.navigate(['/authentication/signin']);
//                 }
//                 this.loading = false;
//               }, 1000);
//             } else {
//               this.error = 'Invalid Login';
//             }
//           },
//           error: (error) => {
//             this.error = error;
//             this.submitted = false;
//             this.loading = false;
//           },
//         });
//     }
//   }
// }
