import { formatDate } from '@angular/common';
import { User } from '@core/service/user.service';

export class Patient {
  id: number;
  userId: number;
  fullName: string;
  email: string;
  phone: string;
  dateOfBirth: Date | string;
  gender: string;
  address: string;
  bloodGroup: string;
  age?: number;
  img?: string;
  user?: User;

  constructor(patient: Partial<Patient> = {}) {
    this.id = patient.id || 0;
    this.userId = patient.userId || 0;
    this.fullName = patient.fullName || '';
    this.email = patient.email || '';
    this.phone = patient.phone || '';
    this.dateOfBirth = patient.dateOfBirth || formatDate(new Date(), 'yyyy-MM-dd', 'en');
    this.gender = patient.gender || 'male';
    this.address = patient.address || '';
    this.bloodGroup = patient.bloodGroup || '';
    this.img = patient.img || 'assets/images/user/user1.jpg';
    this.user = patient.user;

    // Calculate age
    if (this.dateOfBirth) {
      const birthDate = new Date(this.dateOfBirth);
      const today = new Date();
      this.age = today.getFullYear() - birthDate.getFullYear();
    } else {
      this.age = 0;
    }
  }
}