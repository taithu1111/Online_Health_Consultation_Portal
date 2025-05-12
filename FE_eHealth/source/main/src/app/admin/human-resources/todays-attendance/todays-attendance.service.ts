import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { TodaysAttendance } from './todays-attendance..model';

@Injectable({
  providedIn: 'root',
})
export class TodaysAttendanceService {
  private readonly API_URL = 'assets/data/todays-attendance.json';
  private dataChange: BehaviorSubject<TodaysAttendance[]> = new BehaviorSubject<
    TodaysAttendance[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  get data(): TodaysAttendance[] {
    return this.dataChange.value;
  }

  /** GET: Fetch all todaysAttendance's data */
  getAllTodays(): Observable<TodaysAttendance[]> {
    return this.httpClient.get<TodaysAttendance[]>(this.API_URL).pipe(
      map((todays) => {
        this.dataChange.next(todays); // Update the data change observable
        return todays;
      }),
      catchError(this.handleError)
    );
  }

  /** POST: Add a new todaysAttendance's data */
  addToday(todaysAttendance: TodaysAttendance): Observable<TodaysAttendance> {
    return this.httpClient
      .post<TodaysAttendance>(this.API_URL, todaysAttendance)
      .pipe(
        map(() => {
          return todaysAttendance; // Return the newly added todaysAttendance's data
        }),
        catchError(this.handleError)
      );
  }

  /** PUT: Update an existing todaysAttendance's data */
  updateToday(
    todaysAttendance: TodaysAttendance
  ): Observable<TodaysAttendance> {
    return this.httpClient
      .put<TodaysAttendance>(`${this.API_URL}`, todaysAttendance)
      .pipe(
        map(() => {
          return todaysAttendance; // Return the updated todaysAttendance's data
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Remove todaysAttendance's data by ID */
  deleteToday(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // Return the ID of the deleted todaysAttendance's data
      }),
      catchError(this.handleError)
    );
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}
