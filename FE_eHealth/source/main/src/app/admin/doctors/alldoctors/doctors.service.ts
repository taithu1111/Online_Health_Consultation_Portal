import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Doctors } from './doctors.model';

@Injectable({
  providedIn: 'root',
})
export class DoctorsService {
  private readonly API_URL = 'assets/data/doctors.json';
  dataChange: BehaviorSubject<Doctors[]> = new BehaviorSubject<Doctors[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all doctors */
  getAllDoctors(): Observable<Doctors[]> {
    return this.httpClient
      .get<Doctors[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new doctor */
  addDoctors(doctors: Doctors): Observable<Doctors> {
    return this.httpClient.post<Doctors>(this.API_URL, doctors).pipe(
      map((response) => {
        return doctors; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing doctor */
  updateDoctors(doctors: Doctors): Observable<Doctors> {
    return this.httpClient.put<Doctors>(`${this.API_URL}`, doctors).pipe(
      map((response) => {
        return doctors; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** DELETE: Remove a doctor by ID */
  deleteDoctors(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted doctor
      }),
      catchError(this.handleError)
    );
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}
