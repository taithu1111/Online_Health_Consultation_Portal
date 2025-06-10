// src/app/admin/services/appointment.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AppointmentDto {
    id: number;
    doctorName: string;
    patientName: string;
    appointmentDateTime: string;
    type: string;
}

@Injectable({
    providedIn: 'root'
})
export class AppointmentService {
    private readonly API_URL = 'http://localhost:5175/api/Appointment';

    constructor(private http: HttpClient) { }

    /** GET: Lấy chi tiết appointment theo ID */
    getAppointmentById(id: number): Observable<AppointmentDto> {
        return this.http.get<AppointmentDto>(`${this.API_URL}/${id}`);
    }
}
