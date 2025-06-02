import { formatDate } from '@angular/common';

export class Doctors {
  id: number;
  userId: number;
  img: string;
  fullName: string;
  email: string;
  bio: string;
  specialization: number[];
  phone: string;
  experienceYears: number;
  consultationFee: number;
  rating: number;
  languages: string;

  constructor(doctors: Partial<Doctors> = {}) {
    this.id = doctors.id || -1;
    this.userId = doctors.userId || -1;
    this.img = doctors.img || 'assets/images/user/user1.jpg';
    this.fullName = doctors.fullName || '';
    this.email = doctors.email || '';
    this.specialization = doctors.specialization || [];
    this.phone = doctors.phone || '';
    this.experienceYears = doctors.experienceYears || 0;
    this.consultationFee = doctors.consultationFee || 0;
    this.rating = doctors.rating || 0;
    this.languages = doctors.languages || '';
    this.bio = doctors.bio || '';
  }

  public getRandomID(): string {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000).toString(16);
    };
    return S4() + S4();
  }
}
