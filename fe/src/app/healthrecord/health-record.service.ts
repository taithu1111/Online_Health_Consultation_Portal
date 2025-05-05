import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {

  private apiUrl = 'http://localhost:5000/api/HealthRecord'; // địa chỉ api
  constructor(private http: HttpClient) { }

  // Lấy danh sách hồ sơ sức khỏe của bệnh nhân
  getHealthRecords(patientId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${patientId}`);
  }

  // Tạo mới hồ sơ sức khỏe
  createHealthRecord(healthRecord: any): Observable<any> {
    return this.http.post(this.apiUrl, healthRecord);
  }

  // Cập nhật hồ sơ sức khỏe
  updateHealthRecord(id: number, healthRecord: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, healthRecord);
  }

  // Xóa hồ sơ sức khỏe
  deleteHealthRecord(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
