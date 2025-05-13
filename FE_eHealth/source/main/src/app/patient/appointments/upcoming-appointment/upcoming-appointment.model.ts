// src/app/patient/appointments/upcoming-appointment/upcoming-appointment.model.ts
import { formatDate } from '@angular/common';

export class UpcomingAppointment {
  id: number;
  doctor: string;
  date: string;
  time: string;
  diagnosis: string;
  type: string;
  status: string;
  notes: string;

  constructor(init?: Partial<UpcomingAppointment>) {
    // init có thể undefined => gán về {}
    const appt = init ?? {};

    this.id = appt.id ?? this.getRandomID();
    this.doctor = appt.doctor ?? '';
    // Nếu appt.date đã có thì dùng, nếu chưa (add mới) thì lấy ngày hôm nay
    this.date = appt.date ?? formatDate(new Date(), 'yyyy-MM-dd', 'en');
    this.time = appt.time ?? '';
    this.diagnosis = appt.diagnosis ?? 'no diagnosis';
    this.type = appt.type ?? 'video';
    this.status = appt.status ?? 'awaiting';
    this.notes = appt.notes ?? '';
  }

  private getRandomID(): number {
    const S4 = () => (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    return parseInt(S4() + S4(), 16);
  }
}
