import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ShiftManagement } from './shift-management.model'; // Assuming you have a model for ShiftManagement

@Injectable({
  providedIn: 'root',
})
export class ShiftManagementService {
  private readonly API_URL = 'assets/data/shift-management.json'; // Mock API URL or endpoint
  dataChange: BehaviorSubject<ShiftManagement[]> = new BehaviorSubject<
    ShiftManagement[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all shift management data */
  getAllShiftDetails(): Observable<ShiftManagement[]> {
    return this.httpClient
      .get<ShiftManagement[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new shift management entry */
  addShiftManagement(
    shiftManagement: ShiftManagement
  ): Observable<ShiftManagement> {
    return this.httpClient
      .post<ShiftManagement>(this.API_URL, shiftManagement)
      .pipe(
        map((response) => {
          return shiftManagement; // Return the newly added shift management data
        }),
        catchError(this.handleError)
      );
  }

  /** PUT: Update an existing shift management entry */
  updateShiftManagement(
    shiftManagement: ShiftManagement
  ): Observable<ShiftManagement> {
    return this.httpClient
      .put<ShiftManagement>(`${this.API_URL}`, shiftManagement)
      .pipe(
        map((response) => {
          return shiftManagement; // Return the updated shift management data
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Delete a shift management entry by ID */
  deleteShiftManagement(id: string): Observable<string> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // Return the ID of the deleted shift management entry
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
