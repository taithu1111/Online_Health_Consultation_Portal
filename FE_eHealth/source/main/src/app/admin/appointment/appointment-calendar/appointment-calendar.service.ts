import { Injectable } from '@angular/core';
import { EventInput } from '@fullcalendar/core';

@Injectable({
  providedIn: 'root',
})
export class AppointmentCalendarService {
  private readonly API_URL = 'assets/data/appointment.json';

  constructor() {}

  async loadEvents(): Promise<EventInput[]> {
    const response = await fetch(this.API_URL);
    const events = await response.json();

    return events.map((event: any) => ({
      id: event.id,
      title: event.doctor,
      start: new Date(event.date), // Make sure to parse the date string
      end: new Date(event.date), // Make sure to parse the date string
      className: event.className,
      groupId: event.groupId,
      details: event.details,
      allDay: event.allDay || false, // Default to false if not provided
    }));
  }
}
