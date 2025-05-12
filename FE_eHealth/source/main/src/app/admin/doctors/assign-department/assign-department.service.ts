import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { AssignDepartment } from './assign-department.model'; // Assuming you have a model for AssignDepartment

@Injectable({
  providedIn: 'root',
})
export class AssignDepartmentService {
  private readonly API_URL = 'assets/data/assign-department.json';
  dataChange: BehaviorSubject<AssignDepartment[]> = new BehaviorSubject<
    AssignDepartment[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all assigned assignDepartment */
  getAllAssignedDoctors(): Observable<AssignDepartment[]> {
    return this.httpClient
      .get<AssignDepartment[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Assign a doctor to a task or department */
  addAssignDepartment(
    assignDepartment: AssignDepartment
  ): Observable<AssignDepartment> {
    return this.httpClient
      .post<AssignDepartment>(this.API_URL, assignDepartment)
      .pipe(
        map((response) => {
          return assignDepartment; // Return the assigned doctor data
        }),
        catchError(this.handleError)
      );
  }

  /** PUT: Update an existing doctor's assignment */
  updateAssignedDoctor(
    assignDepartment: AssignDepartment
  ): Observable<AssignDepartment> {
    return this.httpClient
      .put<AssignDepartment>(`${this.API_URL}`, assignDepartment)
      .pipe(
        map((response) => {
          return assignDepartment; // Return the updated assignment
        }),
        catchError(this.handleError)
      );
  }

  /** DELETE: Unassign a doctor by ID */
  deleteAssignDepartment(id: string): Observable<string> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // Return the ID of the unassigned doctor
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
