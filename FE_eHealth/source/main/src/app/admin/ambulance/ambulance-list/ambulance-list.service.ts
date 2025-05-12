import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { AmbulanceList } from './ambulance-list.model';

@Injectable({
  providedIn: 'root',
})
export class AmbulanceListService {
  private readonly API_URL = 'assets/data/ambulanceList.json';
  dataChange: BehaviorSubject<AmbulanceList[]> = new BehaviorSubject<
    AmbulanceList[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all ambulance lists */
  getAllAmbulanceLists(): Observable<AmbulanceList[]> {
    return this.httpClient
      .get<AmbulanceList[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new ambulance list */
  addAmbulanceList(ambulanceList: AmbulanceList): Observable<AmbulanceList> {
    return this.httpClient
      .post<AmbulanceList>(this.API_URL, ambulanceList)
      .pipe(
        map((response) => {
          return ambulanceList; // return response from API
        }),
        catchError(this.handleError)
      );
  }

  /** PUT: Update an existing ambulance list */
  updateAmbulanceList(ambulanceList: AmbulanceList): Observable<AmbulanceList> {
    return this.httpClient
      .put<AmbulanceList>(`${this.API_URL}`, ambulanceList)
      .pipe(
        map((response) => {
          return ambulanceList; // return response from API
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Remove an ambulance list by ID */
  deleteAmbulanceList(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted ambulance call list
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
