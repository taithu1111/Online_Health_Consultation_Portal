import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserEnviroment } from 'environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AppointmentService {
    private apiUrl = `${UserEnviroment.apiUrl}/appointment`;

    constructor(private http: HttpClient) { }

    // POST: Tạo cuộc hẹn
    createAppointment(payload: any): Observable<number> {
        return this.http.post<number>(`${this.apiUrl}`, payload);
    }

    // PUT: Cập nhật cuộc hẹn
    updateAppointment(id: number, payload: any, p0: { responseType: string; }): Observable<string> {
        return this.http.put<string>(`${this.apiUrl}/${id}`, payload, { responseType: 'text' as 'json' });
    }

    // DELETE: Hủy cuộc hẹn
    deleteAppointment(id: number): Observable<string> {
        return this.http.delete<string>(`${this.apiUrl}/${id}`);
    }

    // GET: Lấy danh sách lịch hẹn theo bệnh nhân
    getAppointmentsByPatientId(patientId: number): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/patient/${patientId}`);
    }

    // GET: Lấy danh sách lịch hẹn theo bác sĩ
    getAppointmentsByDoctorId(doctorId: number): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/doctor/${doctorId}`);
    }

    // GET: Lấy chi tiết lịch hẹn theo ID
    getAppointmentById(id: number): Observable<any> {
        return this.http.get<any>(`${this.apiUrl}/${id}`);
    }
}
