import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Doctors } from './doctors.model';
import { DoctorEnvironment } from 'environments/environment';
import { PaginatedResponse, PaginationFilter, PaginationParams } from '@core/models/pagination.model';

@Injectable({
  providedIn: 'root',
})
export class DoctorsService {
  private readonly API_URL = DoctorEnvironment.apiUrl;
  dataChange: BehaviorSubject<Doctors[]> = new BehaviorSubject<Doctors[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all doctors with pagination and filters */
  getPaginatedDoctors(
    pagination: PaginationParams,
    filters?: PaginationFilter
  ): Observable<PaginatedResponse<Doctors>> {
    let params = new HttpParams()
      .set('page', pagination.page.toString())
      .set('pageSize', pagination.pageSize.toString());

    if (filters?.specializationId) {
      params = params.set('specializationId', filters.specializationId.toString());
    }
    if (filters?.minExperienceYears) {
      params = params.set('minExperienceYears', filters.minExperienceYears.toString());
    }
    if (filters?.language) {
      params = params.set('language', filters.language);
    }

    return this.httpClient.get<PaginatedResponse<Doctors>>(this.API_URL, { params })
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new doctor */
  addDoctors(doctors: Doctors): Observable<Doctors> {
    return this.httpClient.post<Doctors>(this.API_URL, doctors)
      .pipe(catchError(this.handleError));
  }

  /** PUT: Update an existing doctor */
  updateDoctors(id: number, doctors: Doctors): Observable<Doctors> {
    return this.httpClient.put<Doctors>(`${this.API_URL}/${id}`, doctors)
      .pipe(catchError(this.handleError));
  }

  /** DELETE: Remove a doctor by ID */
  deleteDoctors(id: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.API_URL}/${id}`)
      .pipe(catchError(this.handleError));
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }
}