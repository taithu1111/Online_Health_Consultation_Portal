import { Component, OnInit, ViewChild } from '@angular/core';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { AppointmentCalendarService } from './appointment-calendar.service';
import { EventInput } from '@fullcalendar/core';
import { MatCardModule } from '@angular/material/card';
import {
  FullCalendarComponent,
  FullCalendarModule,
} from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { Router } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
    selector: 'app-appointment-calendar',
    imports: [
        BreadcrumbComponent,
        MatCardModule,
        FullCalendarModule,
        MatTooltipModule,
    ],
    templateUrl: './appointment-calendar.component.html',
    styleUrls: ['./appointment-calendar.component.scss']
})
export class AppointmentCalendarComponent implements OnInit {
  calendarEvents?: EventInput[];
  @ViewChild('calendar', { static: false })
  calendarComponent: FullCalendarComponent | null = null;

  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    weekends: true,
    events: [], // Events will be populated dynamically from the service
    editable: true, // Allow editing events
    selectable: true, // Enable selection for creating events
    dateClick: this.handleDateClick.bind(this), // Handle date click event
    eventClick: this.handleEventClick.bind(this), // Add eventClick handler
    eventContent: this.eventContent.bind(this), // Event content customization
  };

  constructor(
    private appointmentCalendarService: AppointmentCalendarService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentCalendarService.loadEvents().then((events) => {
      this.calendarEvents = events;
      this.calendarOptions.events = this.calendarEvents;

      if (this.calendarComponent) {
        this.calendarComponent.getApi().refetchEvents(); // Calls FullCalendar's refetchEvents method
      }
    });
  }

  handleDateClick(info: any) {}
  handleEventClick(info: any) {
    // Get the clicked event's details
    const eventData = info.event;
    const eventDetails = {
      title: eventData.title,
      start: eventData.start.toISOString(), // Convert to ISO string for passing
      end: eventData.end?.toISOString(), // Ensure the event has an end date
      description: eventData.extendedProps.description,
    };

    // Navigate to the view-appointment page and pass event data as query parameters
    this.router.navigate(['/admin/appointment/viewAppointment'], {
      queryParams: eventDetails,
    });
  }

  eventContent(info: any) {
    const { event } = info;
    // Return basic HTML content for the event
    return {
      html: `
        <div class="fc-event-title">
          ${event.title}
        </div>
      `,
    };
  }
}
