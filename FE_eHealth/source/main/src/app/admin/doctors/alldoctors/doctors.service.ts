import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Doctors } from './doctors.model';
import { DoctorEnvironment } from 'environments/environment';
import { PaginatedResponse, PaginationFilter, PaginationParams } from '@core/models/pagination.model';
import { UpdateUserProfileDto, UserService } from '@core/service/user.service';

@Injectable({
  providedIn: 'root',
})
export class DoctorsService {
  private apiUrl = DoctorEnvironment.apiUrl;
  dataChange: BehaviorSubject<Doctors[]> = new BehaviorSubject<Doctors[]>([]);

  constructor(private httpClient: HttpClient, private userService: UserService) {}

  /** GET: Fetch all doctors with pagination and filters */
  getPaginatedDoctors(
    pagination: PaginationParams,
    filters?: PaginationFilter
  ): Observable<PaginatedResponse<Doctors>> {
    let params = new HttpParams()
      .set('page', pagination.page.toString())
      .set('pageSize', pagination.pageSize.toString());

    filters?.specializations.forEach(spec => {
      params = params.append('specializations', spec.toString());
    });

    if (filters?.minExperienceYears) {
      params = params.set('minExperienceYears', filters.minExperienceYears.toString());
    }
    if (filters?.language) {
      params = params.set('language', filters.language);
    }
    if (filters?.strictSpecializationFilter) {
      params = params.set('strictSpecializationFilter', filters.strictSpecializationFilter.toString());
    }
    return this.httpClient
      .get<PaginatedResponse<Doctors>>(`${this.apiUrl}`, { params })
      .pipe(catchError(this.handleError));
  }

  /** PUT: Update a doctor using the user profile update endpoint */
  updateDoctor(doctor: Doctors): Observable<Doctors> {
    const profileData: UpdateUserProfileDto = {
      fullName: doctor.fullName,
      phone: doctor.phone,
      specialization: doctor.specialization,
      experienceYears: doctor.experienceYears,
      consultationFee: doctor.consultationFee,
      languages: doctor.languages,
      bio: doctor.bio
    };

    return this.userService.updateProfile(profileData).pipe(
      map(() => doctor),
      catchError(this.handleError)
    );
  }

  /** DELETE: Remove one or more doctors by IDs */
  deleteDoctors(id: number | number[]): Observable<void> {
    if (Array.isArray(id)) {
      // POST to a custom bulk-delete endpoint (you must create this endpoint in your backend)
      return this.httpClient
        .post<void>(`${this.apiUrl}/bulk-delete`, { ids: id })
        .pipe(catchError(this.handleError));
    }

    // Single delete fallback
    return this.httpClient
      .delete<void>(`${this.apiUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }
}
