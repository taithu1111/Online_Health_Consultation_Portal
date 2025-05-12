import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { PastAppointment } from './past-appointment.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class PastAppointmentService {
  private readonly API_URL = 'assets/data/past-appointment.json';

  constructor(private httpClient: HttpClient) { }

  /** CRUD METHODS */
  getAllPastAppointments(): Observable<PastAppointment[]> {
    return this.httpClient
      .get<PastAppointment[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  addPastAppointment(
    appointment: PastAppointment
  ): Observable<PastAppointment> {
    return this.httpClient
      .post<PastAppointment>(this.API_URL, appointment)
      .pipe(catchError(this.handleError));
  }

  updatePastAppointment(
    appointment: PastAppointment
  ): Observable<PastAppointment> {
    return this.httpClient
      .put<PastAppointment>(`${this.API_URL}`, appointment)
      .pipe(catchError(this.handleError));
  }

  deletePastAppointment(id: number): Observable<void> {
    return this.httpClient
      .delete<void>(`${this.API_URL}`)
      .pipe(catchError(this.handleError));
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error(`Error: ${error.message}`);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}
