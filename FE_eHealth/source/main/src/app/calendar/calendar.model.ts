import { formatDate } from '@angular/common';
import { Data } from '@angular/router';
export class Calendar {
  id: string;
  title: string;
  category: string;
  startDate: string;
  endDate: string;
  details: string;

  constructor(calendar: Calendar) {
    {
      this.id = calendar.id || this.getRandomID();
      this.title = calendar.title || '';
      this.category = calendar.category || '';
      this.startDate = calendar.startDate
        ? new Date(calendar.startDate).toISOString()
        : new Date().toISOString();
      this.endDate = calendar.endDate
        ? new Date(calendar.endDate).toISOString()
        : new Date().toISOString();
      this.details = calendar.details || '';
    }
  }
  public getRandomID(): string {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return (S4() + S4()).toString(); // Convert to string
  }
}
export interface ScheduleDto {
  id: number;
  doctorId: number;
  // dayOfWeek: number;       // 0 = Sunday … 6 = Saturday
  date: string;         // "yyyy-MM-dd"
  startTime: string;       // "HH:mm:ss"
  endTime: string;         // "HH:mm:ss"
  isAvailable: boolean;
  location: string;
  description: string;
}

export interface AvailableSlotDto {
  slotStart: string;
  slotEnd: string;
}

// Payload cho POST api/schedules
export interface CreateScheduleCommand {
  doctorId: number;
  // dayOfWeek: number;       // 0 = Sunday … 6 = Saturday
  date: string;         // "yyyy-MM-dd"
  startTime: string;       // "HH:mm:ss"
  endTime: string;         // "HH:mm:ss"
  location?: string;
  description?: string;
}

// Payload cho PUT api/schedules/{id}
export interface UpdateScheduleCommand {
  id: number;
  // dayOfWeek: number;
  date?: string;         // "yyyy-MM-dd"
  startTime: string;
  endTime: string;
  location?: string;
  description?: string;
}