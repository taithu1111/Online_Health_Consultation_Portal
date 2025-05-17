import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, of, tap, throwError } from 'rxjs';
import { User } from '../models/user';
import { JwtPayload } from '../models/jwtPayload';
import { jwtDecode } from 'jwt-decode';
import { environment } from 'environments/environment';
import { Role } from '@core/models/role';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;

  public redirectUrl: string | null = null;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());
    this.currentUser = this.currentUserSubject.asObservable();
  }

  private getUserFromStorage(): User | null {
    const userJson = localStorage.getItem('currentUser') || sessionStorage.getItem('currentUser');
    return userJson ? JSON.parse(userJson) : null;
  }

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  // isTokenExpired(): boolean {
  //   const token = this.getToken();
  //   if (!token) return true;

  //   try {
  //     const decoded = jwtDecode<JwtPayload>(token);
  //     const now = Math.floor(Date.now() / 1000);
  //     return decoded.exp <= now;
  //   } catch {
  //     return true;
  //   }
  // }

  isTokenExpired(): boolean {
    return !this.isLoggedIn;
  }

  login(email: string, password: string, rememberMe: boolean = false): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/api/auth/login`, { email, password, })
      .pipe(
        tap(user => {
          if (user?.token) {
            const storage = rememberMe ? localStorage : sessionStorage;
            storage.setItem('currentUser', JSON.stringify(user)),
              this.currentUserSubject.next(user);
          }
        }),
        catchError(err => throwError(() => new Error(err.error?.message || 'Login failed.')))
      );
  }

  logout(): Observable<any> {
    localStorage.removeItem('currentUser');
    sessionStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    return this.http.post(`${environment.apiUrl}/api/auth/logout`, {});
  }

  getDecodedToken(): JwtPayload | null {
    const userJson = localStorage.getItem('currentUser') || sessionStorage.getItem('currentUser');
    if (!userJson) return null;

    try {
      const user: User = JSON.parse(userJson);
      if (!user || !user.token) return null;
      return jwtDecode<JwtPayload>(user.token);
    } catch {
      return null;
    }
  }

  isLoggedIn(): boolean {
    // Check both localStorage and sessionStorage for currentUser
    // const userJson = localStorage.getItem('currentUser') || sessionStorage.getItem('currentUser');
    // if (!userJson) return false;

    const user = this.getUserFromStorage();
    if (!user?.token) return false;

    try {
      const decoded = jwtDecode<JwtPayload>(user.token);
      return decoded.exp > Date.now() / 1000;
      // const user: User = JSON.parse(userJson);
      // if (!user || !user.token) return false;

      // // Verify token is not expired
      // const decoded = jwtDecode<JwtPayload>(user.token);
      // const now = Math.floor(Date.now() / 1000);
      // return decoded.exp > now;
    } catch {
      return false;
    }
  }

  getToken(): string | null {
    // return localStorage.getItem('access_token');
    return this.getUserFromStorage()?.token || null;
  }

  register(registerData: {
    fullName: string;
    email: string;
    password: string;
    confirmPassword: string;
    gender: string;
    role: Role;
    createdAt: string;
  }): Observable<boolean> {
    return this.http.post<boolean>(`${environment.apiUrl}/api/auth/register`, {
      ...registerData,
      createdAt: new Date().toISOString()
    }, {
      responseType: 'text' as 'json'
    }).pipe(
      catchError(err => {
        const errorMessage = err.error?.message || 'Registration failed.';
        console.error('Registration error:', errorMessage, err); // Log the error for debugging
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  forgotPassword(email: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/auth/forgot-password`, { email });
  }
}


// import { Injectable } from '@angular/core';
// import { HttpClient, HttpResponse } from '@angular/common/http';
// import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
// import { User } from '../models/user';
// import { Role } from '@core/models/role';

// @Injectable({
//   providedIn: 'root',
// })
// export class AuthService {
//   private currentUserSubject: BehaviorSubject<User>;
//   public currentUser: Observable<User>;

//   private users = [
//     {
//       id: 1,
//       img: 'assets/images/user/admin.jpg',
//       username: 'admin@hospital.org',
//       password: 'admin@123',
//       firstName: 'Sarah',
//       lastName: 'Smith',
//       role: Role.Admin,
//       token: 'admin-token',
//     },
//     {
//       id: 2,
//       img: 'assets/images/user/doctor.jpg',
//       username: 'doctor@hospital.org',
//       password: 'doctor@123',
//       firstName: 'Ashton',
//       lastName: 'Cox',
//       role: Role.Doctor,
//       token: 'doctor-token',
//     },
//     {
//       id: 3,
//       img: 'assets/images/user/patient.jpg',
//       username: 'patient@hospital.org',
//       password: 'patient@123',
//       firstName: 'Cara',
//       lastName: 'Stevens',
//       role: Role.Patient,
//       token: 'patient-token',
//     },
//   ];

//   constructor(private http: HttpClient) {
//     this.currentUserSubject = new BehaviorSubject<User>(
//       JSON.parse(localStorage.getItem('currentUser') || '{}')
//     );
//     this.currentUser = this.currentUserSubject.asObservable();
//   }

//   public get currentUserValue(): User {
//     return this.currentUserSubject.value;
//   }

//   login(username: string, password: string) {

//     const user = this.users.find((u) => u.username === username && u.password === password);

//     if (!user) {
//       return this.error('Username or password is incorrect');
//     } else {
//       localStorage.setItem('currentUser', JSON.stringify(user));
//       this.currentUserSubject.next(user);
//       return this.ok({
//         id: user.id,
//         img: user.img,
//         username: user.username,
//         firstName: user.firstName,
//         lastName: user.lastName,
//         token: user.token,
//       });
//     }
//   }
//   ok(body?: {
//     id: number;
//     img: string;
//     username: string;
//     firstName: string;
//     lastName: string;
//     token: string;
//   }) {
//     return of(new HttpResponse({ status: 200, body }));
//   }
//   error(message: string) {
//     return throwError(message);
//   }

//   logout() {
//     // remove user from local storage to log user out
//     localStorage.removeItem('currentUser');
//     this.currentUserSubject.next(this.currentUserValue);
//     return of({ success: false });
//   }
// }
