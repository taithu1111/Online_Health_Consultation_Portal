import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Patient } from './patient.model';
import { UpdateUserProfileDto, UserService } from '@core/service/user.service';
import { UserWithProfile } from '@core/models/userWithProfile';
import { UserEnviroment } from 'environments/environment';

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
            phone: userWithProfile.patient?.phone || userWithProfile.user.phoneNumber,
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
    const profileData: UpdateUserProfileDto = {
      fullName: patient.fullName,
      gender: patient.gender,
      dateOfBirth: patient.dateOfBirth,
      bloodType: patient.bloodType,
      phone: patient.phone,
      address: patient.address
    };

    return this.userService.updateProfile(profileData).pipe(
      map(() => patient),
      catchError(this.handleError)
    );
  }

  deletePatient(userId: number): Observable<void> {
    return this.userService.deleteUser(userId).pipe(
      catchError(this.handleError)        // <- reuse the shared handler
    );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }

  private generateId(): number {
    return Math.floor(Math.random() * 10000);
  }

  getPatientById(id: number): Observable<Patient> {
    return this.userService.getUserById(id).pipe(
      map(user => new Patient({
        id: user.id,
        userId: user.id,
        fullName: user.fullName,
        email: user.email,
        phone: user.phoneNumber,
        gender: user.gender,
        dateOfBirth: user.dateOfBirth,
        bloodType: user.bloodType,
        address: user.address,
        img: 'assets/images/user/user1.jpg',
        user: user
      })),
      catchError(this.handleError)
    );
  }
}