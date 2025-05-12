import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { AmbulanceCallList } from './ambulance-call-list.model';

@Injectable({
  providedIn: 'root',
})
export class AmbulanceCallListService {
  private readonly API_URL = 'assets/data/ambulanceCallList.json';
  dataChange: BehaviorSubject<AmbulanceCallList[]> = new BehaviorSubject<
    AmbulanceCallList[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all ambulance call lists */
  getAllAmbulanceCallLists(): Observable<AmbulanceCallList[]> {
    return this.httpClient
      .get<AmbulanceCallList[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new ambulance call list */
  addAmbulanceCallList(
    ambulanceCallList: AmbulanceCallList
  ): Observable<AmbulanceCallList> {
    return this.httpClient
      .post<AmbulanceCallList>(this.API_URL, ambulanceCallList)
      .pipe(
        map((response) => {
          return ambulanceCallList; // return response from API
        }),
        catchError(this.handleError)
      );
  }

  /** PUT: Update an existing ambulance call list */
  updateAmbulanceCallList(
    ambulanceCallList: AmbulanceCallList
  ): Observable<AmbulanceCallList> {
    return this.httpClient
      .put<AmbulanceCallList>(`${this.API_URL}`, ambulanceCallList)
      .pipe(
        map((response) => {
          return ambulanceCallList; // Return updated staff
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Remove an ambulance call list by ID */
  deleteAmbulanceCallList(id: number): Observable<number> {
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
