import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { BloodIssued } from './blood-issued.model';

@Injectable({
  providedIn: 'root',
})
export class BloodIssuedService {
  private readonly API_URL = 'assets/data/blood-issued.json';
  dataChange: BehaviorSubject<BloodIssued[]> = new BehaviorSubject<
    BloodIssued[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  getAllBloodIssued(): Observable<BloodIssued[]> {
    return this.httpClient
      .get<BloodIssued[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  addBloodIssued(bloodIssued: BloodIssued): Observable<BloodIssued> {
    return this.httpClient.post<BloodIssued>(this.API_URL, bloodIssued).pipe(
      map(() => bloodIssued),
      catchError(this.handleError)
    );
  }

  updateBloodIssued(bloodIssued: BloodIssued): Observable<BloodIssued> {
    return this.httpClient
      .put<BloodIssued>(`${this.API_URL}`, bloodIssued)
      .pipe(
        map(() => bloodIssued),
        catchError(this.handleError)
      );
  }

  deleteBloodIssued(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted blood issued
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
