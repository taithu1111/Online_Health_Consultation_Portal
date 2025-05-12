import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Appointments } from './appointments.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AppointmentsService {
  private readonly API_URL = 'assets/data/doc-appointments.json';
  private isTblLoading = true;
  private dataChange: BehaviorSubject<Appointments[]> = new BehaviorSubject<
    Appointments[]
  >([]);
  private dialogData!: Appointments;

  constructor(private httpClient: HttpClient) {}

  get data(): Appointments[] {
    return this.dataChange.value;
  }

  getDialogData(): Appointments {
    return this.dialogData;
  }

  /** CRUD METHODS */
  getAllAppointments(): Observable<Appointments[]> {
    this.isTblLoading = true;
    return this.httpClient
      .get<Appointments[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  addAppointments(appointments: Appointments): Observable<Appointments> {
    return this.httpClient
      .post<Appointments>(this.API_URL, appointments)
      .pipe(catchError(this.handleError));
  }

  updateAppointments(appointments: Appointments): Observable<Appointments> {
    return this.httpClient
      .put<Appointments>(`${this.API_URL}`, appointments)
      .pipe(catchError(this.handleError));
  }

  deleteAppointments(id: number): Observable<void> {
    return this.httpClient
      .delete<void>(`${this.API_URL}`)
      .pipe(catchError(this.handleError));
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}
