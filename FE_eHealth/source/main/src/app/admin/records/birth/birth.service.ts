import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Birth } from './birth.model';

@Injectable({
  providedIn: 'root',
})
export class BirthService {
  private readonly API_URL = 'assets/data/birth.json';
  dataChange: BehaviorSubject<Birth[]> = new BehaviorSubject<Birth[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all birth records */
  getAllBirths(): Observable<Birth[]> {
    return this.httpClient
      .get<Birth[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new birth record */
  addBirth(birth: Birth): Observable<Birth> {
    return this.httpClient.post<Birth>(this.API_URL, birth).pipe(
      map(() => {
        return birth; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing birth record */
  updateBirth(birth: Birth): Observable<Birth> {
    return this.httpClient.put<Birth>(`${this.API_URL}`, birth).pipe(
      map(() => {
        return birth; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** DELETE: Remove a birth record by ID */
  deleteBirth(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // return the ID of the deleted birth record
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
