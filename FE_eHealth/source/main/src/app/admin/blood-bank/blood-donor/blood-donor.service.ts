import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { BloodDonor } from './blood-donor.model';

@Injectable({
  providedIn: 'root',
})
export class BloodDonorService {
  private readonly API_URL = 'assets/data/blood-donors.json';
  dataChange: BehaviorSubject<BloodDonor[]> = new BehaviorSubject<BloodDonor[]>(
    []
  );

  constructor(private httpClient: HttpClient) {}

  getAllBloodDonors(): Observable<BloodDonor[]> {
    return this.httpClient
      .get<BloodDonor[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  addBloodDonor(bloodDonor: BloodDonor): Observable<BloodDonor> {
    return this.httpClient.post<BloodDonor>(this.API_URL, bloodDonor).pipe(
      map(() => bloodDonor),
      catchError(this.handleError)
    );
  }

  updateBloodDonor(bloodDonor: BloodDonor): Observable<BloodDonor> {
    return this.httpClient.put<BloodDonor>(`${this.API_URL}`, bloodDonor).pipe(
      map(() => bloodDonor),
      catchError(this.handleError)
    );
  }

  deleteBloodDonor(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted blood donor
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
