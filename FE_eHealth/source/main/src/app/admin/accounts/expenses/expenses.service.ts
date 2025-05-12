import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Expenses } from './expenses.model'; // Assuming you have an Expenses model like the Expenses model

@Injectable({
  providedIn: 'root',
})
export class ExpensesService {
  private readonly API_URL = 'assets/data/expenses.json'; // Adjust API URL as needed
  dataChange: BehaviorSubject<Expenses[]> = new BehaviorSubject<Expenses[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all expense records */
  getAllExpensesRecords(): Observable<Expenses[]> {
    return this.httpClient
      .get<Expenses[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new expense record */
  addExpensesRecord(expense: Expenses): Observable<Expenses> {
    return this.httpClient.post<Expenses>(this.API_URL, expense).pipe(
      map(() => {
        return expense; // return the expense record after adding
      }),
      catchError(this.handleError)
    );
  }

  /** PUT: Update an existing expense record */
  updateExpensesRecord(expense: Expenses): Observable<Expenses> {
    return this.httpClient.put<Expenses>(`${this.API_URL}`, expense).pipe(
      map(() => {
        return expense; // return the updated expense record
      }),
      catchError(this.handleError)
    );
  }

  /** DELETE: Remove an expense record by ID */
  deleteExpensesRecord(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map(() => {
        return id; // return the ID of the deleted expense record
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
