import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  ScheduleDto,
  AvailableSlotDto,
  CreateScheduleCommand,
  UpdateScheduleCommand
} from './calendar.model';

@Injectable({ providedIn: 'root' })
export class CalendarService {
  private readonly baseUrl = 'api/schedules';

  getDoctorSchedules(doctorId: number): Observable<ScheduleDto[]> {
    return this.http.get<ScheduleDto[]>(`${this.baseUrl}/doctor/${doctorId}`);
  }

  getAvailableSlots(doctorId: number, date: string): Observable<AvailableSlotDto[]> {
    // query string phải là DoctorId & Date :contentReference[oaicite:16]{index=16}:contentReference[oaicite:17]{index=17}
    const params = new HttpParams()
      .set('DoctorId', doctorId.toString())
      .set('Date', date);
    return this.http.get<AvailableSlotDto[]>(`${this.baseUrl}/available-slots`, { params });
  }

  createSchedule(cmd: CreateScheduleCommand): Observable<number> {
    // POST api/schedules trả về int Id :contentReference[oaicite:18]{index=18}:contentReference[oaicite:19]{index=19}
    return this.http.post<number>(this.baseUrl, cmd);
  }

  updateSchedule(id: number, cmd: UpdateScheduleCommand): Observable<boolean> {
    // PUT api/schedules/{id} trả về bool :contentReference[oaicite:20]{index=20}:contentReference[oaicite:21]{index=21}
    return this.http.put<boolean>(`${this.baseUrl}/${id}`, cmd);
  }

  deleteSchedule(id: number): Observable<boolean> {
    // DELETE api/schedules/{id} trả về bool :contentReference[oaicite:22]{index=22}:contentReference[oaicite:23]{index=23}
    return this.http.delete<boolean>(`${this.baseUrl}/${id}`);
  }

  constructor(private http: HttpClient) { }
}