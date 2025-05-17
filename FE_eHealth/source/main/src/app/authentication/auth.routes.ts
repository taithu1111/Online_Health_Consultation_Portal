import { Route } from "@angular/router";
import { SigninComponent } from "./signin/signin.component";
import { SignupComponent } from "./signup/signup.component";
import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
import { LockedComponent } from "./locked/locked.component";
import { Page404Component } from "./page404/page404.component";
import { Page500Component } from "./page500/page500.component";
import { AuthGuard } from "@core/guard/auth.guard";
import { Role } from "@core";
// import { ResetPasswordComponent } from "./reset-password/reset-password.component";
export const AUTH_ROUTE: Route[] = [
  {
    path: "",
    redirectTo: "signin",
    pathMatch: "full",
  },
  {
    path: "signin",
    component: SigninComponent,
  },
  {
    path: "signup",
    component: SignupComponent,
  },
  {
    path: "forgot-password",
    component: ForgotPasswordComponent,
  },
  {
    path: "locked",
    component: LockedComponent,
  },
  {
    path: "page404",
    component: Page404Component,
  },
  {
    path: "page500",
    component: Page500Component,
  },
  // {
  //   path: "reset-password",
  //   component: ResetPasswordComponent,
  // }
];

export const APP_ROUTES: Route[] = [
  {
    path: 'admin',
    canActivate: [AuthGuard],
    data: { roles: [Role.Admin] },
    loadChildren: () => import('../admin/dashboard/main/main.component').then(m => m.MainComponent)
  },
  {
    path: 'doctor',
    canActivate: [AuthGuard],
    data: { roles: [Role.Doctor] },
    loadChildren: () => import('../doctor/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'patient',
    canActivate: [AuthGuard],
    data: { roles: [Role.Patient] },
    loadChildren: () => import('../patient/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'authentication',
    children: AUTH_ROUTE
  },
  {
    path: '**',
    redirectTo: 'authentication/page404'
  }
];
// import { Route } from "@angular/router";
// import { SigninComponent } from "./signin/signin.component";
// import { SignupComponent } from "./signup/signup.component";
// import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
// import { LockedComponent } from "./locked/locked.component";
// import { Page404Component } from "./page404/page404.component";
// import { Page500Component } from "./page500/page500.component";
// export const AUTH_ROUTE: Route[] = [
//   {
//     path: "",
//     redirectTo: "signin",
//     pathMatch: "full",
//   },
//   {
//     path: "signin",
//     component: SigninComponent,
//   },
//   {
//     path: "signup",
//     component: SignupComponent,
//   },
//   {
//     path: "forgot-password",
//     component: ForgotPasswordComponent,
//   },
//   {
//     path: "locked",
//     component: LockedComponent,
//   },
//   {
//     path: "page404",
//     component: Page404Component,
//   },
//   {
//     path: "page500",
//     component: Page500Component,
//   },
// ];
