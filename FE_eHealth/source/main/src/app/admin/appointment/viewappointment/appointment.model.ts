import { formatDate } from '@angular/common';

export class Appointment {
  id: number;
  // img: string;
  patientName: string;
  doctorName: string;
  appointmentDateTime: string;
  email: string;
  gender: string;
  phone: string;
  address: string;
  status: string;
  type: string;
  notes: string;
  diagnosis: string;


  constructor(appointment: Partial<Appointment>) {
    this.id = appointment.id || this.getRandomID();
    // this.img = appointment.img || 'assets/images/user/user1.jpg';
    this.patientName = appointment.patientName || '';
    this.doctorName = appointment.doctorName || '';
    this.appointmentDateTime = appointment.appointmentDateTime || formatDate(new Date(), 'yyyy-MM-ddTHH:mm:ss', 'en-US');
    this.email = appointment.email || '';
    this.gender = appointment.gender || '';
    this.phone = appointment.phone || '';
    this.address = appointment.address || '';
    this.status = appointment.status || 'Pending';
    this.type = appointment.type || 'New Patient';
    this.notes = appointment.notes || '';
    this.diagnosis = appointment.diagnosis || '';
  }

  public getRandomID(): number {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return S4() + S4();
  }
}
