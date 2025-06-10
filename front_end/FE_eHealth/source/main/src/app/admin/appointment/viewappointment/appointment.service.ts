import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Appointment } from './appointment.model';

@Injectable({
  providedIn: 'root',
})
export class AppointmentService {
  private readonly API_URL = 'http://localhost:5175/api/Appointment';
  dataChange: BehaviorSubject<Appointment[]> = new BehaviorSubject<
    Appointment[]
  >([]);

  constructor(private httpClient: HttpClient) { }
  /** GET: Fetch all appointments */
  getAllAppointments(): Observable<Appointment[]> {
    return this.httpClient
      .get<Appointment[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new appointment */
  addAppointment(appointment: Appointment): Observable<Appointment> {
    return this.httpClient.post<Appointment>(this.API_URL, appointment).pipe(
      map((response) => {
        return appointment; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing appointment */
  updateAppointment(appointment: Appointment): Observable<Appointment> {
    return this.httpClient.put<Appointment>(`${this.API_URL}`, appointment)
      .pipe(
        map(response => response),
        catchError(this.handleError)
      );
  }

  /** DELETE: Remove an appointment by ID */
  deleteAppointment(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted appointment
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
