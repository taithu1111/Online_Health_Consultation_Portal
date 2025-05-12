import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ClaimStatus } from './claim-status.model';

@Injectable({
  providedIn: 'root',
})
export class ClaimStatusService {
  private readonly API_URL = 'assets/data/claim-status.json'; // Replace with actual API endpoint if needed
  dataChange: BehaviorSubject<ClaimStatus[]> = new BehaviorSubject<
    ClaimStatus[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all claim status data */
  getAllClaimStatus(): Observable<ClaimStatus[]> {
    return this.httpClient
      .get<ClaimStatus[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new claim status record */
  addClaimStatus(claimStatus: ClaimStatus): Observable<ClaimStatus> {
    return this.httpClient.post<ClaimStatus>(this.API_URL, claimStatus).pipe(
      map((response) => {
        return claimStatus; // Return the newly added claim status object
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing claim status record */
  updateClaimStatus(claimStatus: ClaimStatus): Observable<ClaimStatus> {
    return this.httpClient
      .put<ClaimStatus>(`${this.API_URL}`, claimStatus)
      .pipe(
        map((response) => {
          return claimStatus; // Return the updated claim status object
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Remove a claim status record by ID */
  deleteClaimStatus(id: string): Observable<string> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // Return the ID of the deleted claim status record
      }),
      catchError(this.handleError)
    );
  }

  /** Handle HTTP operation that failed */
  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}
