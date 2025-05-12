import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { DepartmentList } from './department-list.model';

@Injectable({
  providedIn: 'root',
})
export class DepartmentListService {
  private readonly API_URL = 'assets/data/departmentList.json';
  dataChange: BehaviorSubject<DepartmentList[]> = new BehaviorSubject<
    DepartmentList[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all department lists */
  getAllDepartmentLists(): Observable<DepartmentList[]> {
    return this.httpClient
      .get<DepartmentList[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new department list */
  addDepartmentList(
    departmentList: DepartmentList
  ): Observable<DepartmentList> {
    return this.httpClient
      .post<DepartmentList>(this.API_URL, departmentList)
      .pipe(
        map((response) => {
          return departmentList; // return the added department list
        }),
        catchError(this.handleError)
      );
  }

  /** PUT: Update an existing department list */
  updateDepartmentList(
    departmentList: DepartmentList
  ): Observable<DepartmentList> {
    return this.httpClient
      .put<DepartmentList>(`${this.API_URL}`, departmentList)
      .pipe(
        map((response) => {
          return departmentList; // return the updated department list
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Remove a department list by ID */
  deleteDepartmentList(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted department list
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
