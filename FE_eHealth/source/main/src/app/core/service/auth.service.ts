import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, of, tap, throwError } from 'rxjs';
import { User } from '../models/user';
import { JwtPayload } from '../models/jwtPayload';
import { jwtDecode } from 'jwt-decode';
import { AuthEnvironment } from 'environments/environment';
import { Role } from '@core/models/role';
import { BloodType } from '@core/models/bloodType';

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

  getUserFromStorage(): User | null {
    const userJson = localStorage.getItem('currentUser') || sessionStorage.getItem('currentUser');
    return userJson ? JSON.parse(userJson) : null;
  }

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  isTokenExpired(): boolean {
    return !this.isLoggedIn;
  }

  getCurrentUserRole(): string | null {
    const user = this.getUserFromStorage();
    if (!user?.token) return null;

    try {
      const decoded: JwtPayload = jwtDecode(user.token);
      if (!decoded || !decoded.role) return null;

      return Array.isArray(decoded.role) ? decoded.role[0] : decoded.role;
    } catch {
      return null;
    }
  }

  login(email: string, password: string, rememberMe: boolean = false): Observable<User> {
    return this.http.post<User>(`${AuthEnvironment.apiUrl}/login`, { email, password })
      .pipe(
        tap(user => {
          if (user?.token) {
            const storage = rememberMe ? localStorage : sessionStorage;
            storage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
          }
        }),
        catchError(err => {
          // Handle specific error messages from the server
          const errorMsg = err.error?.message || 'Login failed. Please check your credentials.';
          return throwError(() => new Error(errorMsg));
        })
      );
  }

  logout(): Observable<any> {
    localStorage.removeItem('currentUser');
    sessionStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    return this.http.post(`${AuthEnvironment.apiUrl}/logout`, {});
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
  //lấy fullname theo token
  getFullNameFromToken(token: string): string {
    try {
      const decoded: any = jwtDecode(token);
      return decoded.fullName || '';
    } catch (error) {
      console.error('Token decode error', error);
      return '';
    }
  }
  get currentToken(): string | null {
    // Lấy token từ localStorage hoặc biến lưu token
    return localStorage.getItem('token');
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
    phoneNumber: string;
    createdAt: string;
    dateOfBirth: Date;
    address: string;
    bloodType: BloodType;
  }): Observable<boolean> {
    console.log('Register URL:', `${AuthEnvironment.apiUrl}/register`);

    return this.http.post<boolean>(`${AuthEnvironment.apiUrl}/register`, {
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
    return this.http.post(`${AuthEnvironment.apiUrl}/forgot-password`, { email });
  }

  resetPassword(email: string, token: string, newPassword: string): Observable<boolean> {
    return this.http.post<boolean>(`${AuthEnvironment.apiUrl}/reset-password`, { 
      email, 
      token, 
      newPassword 
    }).pipe(
      catchError(err => {
        const errorMessage = err.error?.message || 'Password reset failed.';
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}