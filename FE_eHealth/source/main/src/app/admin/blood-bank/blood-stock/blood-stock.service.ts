import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { BloodStock } from './blood-stock.model';

@Injectable({
  providedIn: 'root',
})
export class BloodStockService {
  private readonly API_URL = 'assets/data/blood-stock.json';
  dataChange: BehaviorSubject<BloodStock[]> = new BehaviorSubject<BloodStock[]>(
    []
  );

  constructor(private httpClient: HttpClient) {}

  getAllBloodStocks(): Observable<BloodStock[]> {
    return this.httpClient
      .get<BloodStock[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  addBloodStock(bloodStock: BloodStock): Observable<BloodStock> {
    return this.httpClient.post<BloodStock>(this.API_URL, bloodStock).pipe(
      map(() => bloodStock),
      catchError(this.handleError)
    );
  }

  updateBloodStock(bloodStock: BloodStock): Observable<BloodStock> {
    return this.httpClient.put<BloodStock>(`${this.API_URL}`, bloodStock).pipe(
      map(() => bloodStock),
      catchError(this.handleError)
    );
  }

  deleteBloodStock(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted doctor
      }),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}
