import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthService } from '../service/auth.service';
import { Role } from '@core/models/role';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(private authService: AuthService, private router: Router) { }

  private isAuthRoute(url: string): boolean {
    const normalizedUrl = url.replace(/^#/, '');
    return normalizedUrl.startsWith('/authentication/') ||
      normalizedUrl === '/authentication' ||
      normalizedUrl.startsWith('/authentication/signin');
  }

  private hasRequiredRole(allowedRoles: Role[]): boolean {
    const user = this.authService.currentUserValue;
    return !!user?.roles?.some(role => allowedRoles.includes(role as Role));
  }

  private redirectBasedOnRole(): void {
    const user = this.authService.currentUserValue;
    if (!user) return;

    const role = user.roles?.[0] as Role;
    const routes = {
      [Role.Admin]: '/admin/dashboard/main',
      [Role.Doctor]: '/doctor/dashboard',
      [Role.Patient]: '/patient/dashboard'
    };

    this.router.navigate([routes[role] || '/authentication/signin']);
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.authService.isLoggedIn()) {
      if (this.isAuthRoute(state.url)) {
        this.redirectBasedOnRole();
        return false;
      }

      const allowedRoles: Role[] = route.data['roles'] || [];
      if (allowedRoles.length > 0 && !this.hasRequiredRole(allowedRoles)) {
        this.router.navigate(['/authentication/page404']);
        return false;
      }

      return true;
    }

    if (!this.isAuthRoute(state.url)) {
      this.authService.redirectUrl = state.url;
      this.router.navigate(['/authentication/signin']);
      return false;
    }

    return true;
    // const currentUser = this.authService.currentUserValue;
    // if (currentUser) {
    //   const userRoles = currentUser.roles;
    //   const allowedRoles: string[] = route.data['role'] || [];

    //   const hasRole = userRoles.some(role => allowedRoles.includes(role));
    //   if (!hasRole && allowedRoles.length > 0) {
    //     this.router.navigate(['/authentication/signin']);
    //     return false;
    //   }

    //   return true;
    // }

    // this.router.navigate(['/authentication/signin']);
    // return false;
  }
}


// import { Injectable } from '@angular/core';
// import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

// import { AuthService } from '../service/auth.service';

// @Injectable({
//   providedIn: 'root',
// })
// export class AuthGuard {
//   constructor(private authService: AuthService, private router: Router) { }

//   // eslint-disable-next-line @typescript-eslint/no-unused-vars
//   canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
//     if (this.authService.currentUserValue) {
//       const userRole = this.authService.currentUserValue.role;
//       if (route.data['role'] && route.data['role'].indexOf(userRole) === -1) {
//         this.router.navigate(['/authentication/signin']);
//         return false;
//       }
//       return true;
//     }

//     this.router.navigate(['/authentication/signin']);
//     return false;
//   }
// }
