import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BillList, PaymentDto } from './bill-list.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BillListService {
  private readonly API_URL = `${environment.apiUrl}/api/payments`;

  constructor(private httpClient: HttpClient) { }

  /** GET: Fetch all bill lists from backend */
  getAllBillLists(): Observable<PaymentDto[]> {
    return this.httpClient.get<PaymentDto[]>(this.API_URL).pipe(
      catchError(this.handleError)
    );
  }

  /** POST: Create a new payment record */
  createBill(payload: Partial<PaymentDto>): Observable<PaymentDto> {
    return this.httpClient.post<PaymentDto>(this.API_URL, payload).pipe(
      catchError(this.handleError)
    );
  }

  /** PUT: Update payment status */
  updateBill(bill: BillList): Observable<any> {
    const url = `${this.API_URL}/${bill.id}`;
    return this.httpClient.put<any>(url, bill); // gửi toàn bộ object
  }

  /** DELETE: Delete a payment */
  deleteBill(id: number): Observable<void> {
    const url = `${this.API_URL}/${id}`;
    return this.httpClient.delete<void>(url).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }
}
