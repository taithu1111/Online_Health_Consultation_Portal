import { formatDate } from '@angular/common';

export class Appointments {
  id: number;
  patientID: number;
  doctorID: number;
  img: string;
  patientName: string;
  email: string;
  phoneNumber: string;
  // appointmentDate: string;
  // appointmentTime: string;
  appointmentDateTime: string;
  gender: string;
  status: string;
  address: string;
  disease: string;
  lastVisitDate: string;

  constructor(appointments: Partial<Appointments>) {
    this.id = appointments.id || this.getRandomID();
    this.patientID = appointments.patientID || this.getRandomID();
    this.doctorID = appointments.doctorID || this.getRandomID();
    this.img = appointments.img || 'assets/images/user/usrbig1.jpg';
    this.patientName = appointments.patientName || '';
    this.email = appointments.email || '';
    this.phoneNumber = appointments.phoneNumber || '';
    // this.appointmentDate =
    //   appointments.appointmentDate ||
    //   formatDate(new Date(), 'yyyy-MM-ddTHH:mm:ssZ', 'en');
    // this.appointmentTime = appointments.appointmentTime || '';
    this.appointmentDateTime = appointments.appointmentDateTime || new Date().toISOString();
    this.gender = appointments.gender || '';
    this.status = appointments.status || 'Upcoming'; // Default status
    this.address = appointments.address || '';
    this.disease = appointments.disease || '';
    this.lastVisitDate =
      appointments.lastVisitDate ||
      formatDate(new Date(), 'yyyy-MM-ddTHH:mm:ssZ', 'en'); // Default to current date
  }

  private getRandomID(): number {
    const S4 = () => (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    return parseInt(S4() + S4(), 16);
  }
}
