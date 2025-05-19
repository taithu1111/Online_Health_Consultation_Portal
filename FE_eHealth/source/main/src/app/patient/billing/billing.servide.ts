import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaymentDto, CreatePaymentCommand } from './billing.model';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class PaymentService {
    private readonly baseUrl = `${environment.apiUrl}/api/payments`;

    constructor(private http: HttpClient) { }

    /**
     * Tạo mới payment
     * POST api/payments
     */
    createPayment(cmd: CreatePaymentCommand): Observable<PaymentDto> {
        return this.http.post<PaymentDto>(this.baseUrl, cmd);
    }

    /**
     * Lấy payment theo id
     * GET api/payments/{id}
     */
    getPaymentById(id: number): Observable<PaymentDto> {
        const url = `${this.baseUrl}/${id}`;
        return this.http.get<PaymentDto>(url);
    }

    /**
     * Lấy danh sách payments theo appointmentId
     * GET api/payments/appointment/{appointmentId}
     */
    getPaymentsByAppointmentId(appointmentId: number): Observable<PaymentDto[]> {
        const url = `${this.baseUrl}/appointment/${appointmentId}`;
        return this.http.get<PaymentDto[]>(url);
    }
}