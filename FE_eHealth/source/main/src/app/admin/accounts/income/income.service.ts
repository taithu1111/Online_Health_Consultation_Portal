import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Income } from './income.model';

@Injectable({
  providedIn: 'root',
})
export class IncomeService {
  private readonly API_URL = 'assets/data/income.json'; // Adjust API URL as needed
  dataChange: BehaviorSubject<Income[]> = new BehaviorSubject<Income[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all income records */
  getAllIncomeRecords(): Observable<Income[]> {
    return this.httpClient
      .get<Income[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new income record */
  addIncomeRecord(income: Income): Observable<Income> {
    return this.httpClient.post<Income>(this.API_URL, income).pipe(
      map(() => {
        return income; // return the income record after adding
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing income record */
  updateIncomeRecord(income: Income): Observable<Income> {
    return this.httpClient.put<Income>(`${this.API_URL}`, income).pipe(
      map(() => {
        return income; // return the updated income record
      }),
      catchError(this.handleError)
    );
  }

  /** DELETE: Remove an income record by ID */
  deleteIncomeRecord(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // return the ID of the deleted income record
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
