import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError, of } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Patient } from './patient.model';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private readonly API_URL = 'assets/data/patient.json';
  dataChange: BehaviorSubject<Patient[]> = new BehaviorSubject<Patient[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all patients */
  getAllPatients(): Observable<Patient[]> {
    return this.httpClient
      .get<Patient[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new patient */
  addPatient(patient: Patient): Observable<Patient> {
    return this.httpClient.post<Patient>(this.API_URL, patient).pipe(
      map((response) => {
        return patient; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing patient */
  updatePatient(patient: Patient): Observable<Patient> {
    return this.httpClient.put<Patient>(`${this.API_URL}`, patient).pipe(
      map((response) => {
        return patient; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** DELETE: Remove a patient by ID */
  deletePatient(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // return the ID of the deleted patient
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
