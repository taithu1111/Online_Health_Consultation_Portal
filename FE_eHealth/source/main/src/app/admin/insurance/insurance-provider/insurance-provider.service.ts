import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { InsuranceProvider } from './insurance-provider.model'; // Import the InsuranceProvider model

@Injectable({
  providedIn: 'root',
})
export class InsuranceProviderService {
  private readonly API_URL = 'assets/data/insurance-providers.json'; // Replace with actual API endpoint if needed
  dataChange: BehaviorSubject<InsuranceProvider[]> = new BehaviorSubject<
    InsuranceProvider[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all insurance providers */
  getAllInsuranceProviders(): Observable<InsuranceProvider[]> {
    return this.httpClient
      .get<InsuranceProvider[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new insurance provider */
  addInsuranceProvider(
    insuranceProvider: InsuranceProvider
  ): Observable<InsuranceProvider> {
    return this.httpClient
      .post<InsuranceProvider>(this.API_URL, insuranceProvider)
      .pipe(
        map((response) => {
          return insuranceProvider; // Return the newly added insurance provider object
        }),
        catchError(this.handleError)
      );
  }

  /** PUT: Update an existing insurance provider */
  updateInsuranceProvider(
    insuranceProvider: InsuranceProvider
  ): Observable<InsuranceProvider> {
    return this.httpClient
      .put<InsuranceProvider>(`${this.API_URL}`, insuranceProvider)
      .pipe(
        map((response) => {
          return insuranceProvider; // Return the updated insurance provider object
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Remove an insurance provider record by ID */
  deleteInsuranceProvider(id: string): Observable<string> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // Return the ID of the deleted insurance provider record
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
