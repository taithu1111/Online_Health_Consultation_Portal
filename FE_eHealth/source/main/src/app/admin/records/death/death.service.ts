import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Death } from './death.model';

@Injectable({
  providedIn: 'root',
})
export class DeathService {
  private readonly API_URL = 'assets/data/death.json';
  dataChange: BehaviorSubject<Death[]> = new BehaviorSubject<Death[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all death records */
  getAllDeaths(): Observable<Death[]> {
    return this.httpClient
      .get<Death[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new death record */
  addDeath(death: Death): Observable<Death> {
    return this.httpClient.post<Death>(this.API_URL, death).pipe(
      map(() => {
        return death; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing death record */
  updateDeath(death: Death): Observable<Death> {
    return this.httpClient.put<Death>(`${this.API_URL}`, death).pipe(
      map(() => {
        return death; // return response from API
      }),
      catchError(this.handleError)
    );
  }

  /** DELETE: Remove a death record by ID */
  deleteDeath(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // return the ID of the deleted death record
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
