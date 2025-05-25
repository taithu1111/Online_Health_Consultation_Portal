import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Patient } from './patient.model';
import { UserService } from '@core/service/user.service';
import { UserWithProfile } from '@core/models/userWithProfile';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  constructor(private http: HttpClient, private userService: UserService) {}

  getAllPatients(page: number, pageSize: number, searchTerm: string): Observable<Patient[]> {
    return this.userService.getUsers(page, pageSize, 'Patient', searchTerm).pipe(
      map(response => response.items.map(userWithProfile => 
        new Patient({
          id: userWithProfile.user.id,
          userId: userWithProfile.user.id,
          fullName: userWithProfile.user.fullName,
          email: userWithProfile.user.email,
          phone: userWithProfile.patient?.phone || '',
          gender: userWithProfile.patient?.gender || '',
          dateOfBirth: userWithProfile.patient?.dateOfBirth || '',
          bloodType: userWithProfile.patient?.bloodType || '',
          address: userWithProfile.patient?.address || '',
          user: userWithProfile.user
        })
      )),
      catchError(this.handleError)
    );
  }

  updatePatient(patient: Patient): Observable<Patient> {
    // In a real implementation, you would call userService.updateProfile()
    return of(patient).pipe(
      catchError(this.handleError)
    );
  }

  deletePatient(id: number): Observable<number> {
    // In a real implementation, you would call userService.deleteUser()
    return of(id).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }

  private generateId(): number {
    return Math.floor(Math.random() * 10000);
  }
}