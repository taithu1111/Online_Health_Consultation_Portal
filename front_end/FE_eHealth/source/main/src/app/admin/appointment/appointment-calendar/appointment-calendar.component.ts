// src/app/admin/appointment/appointment-calendar/appointment-calendar.component.ts
import { Component, OnInit, ViewChild } from '@angular/core';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { CalendarOptions, EventInput } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { Router } from '@angular/router';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { UserEnviroment } from 'environments/environment';
import { AppointmentCalendarService, Appointment } from '../appointment-calendar/appointment-calendar.service';

@Component({
  selector: 'app-appointment-calendar',
  standalone: true,
  imports: [
    BreadcrumbComponent,
    FullCalendarModule,
    CommonModule,
    MatCardModule,
  ],
  templateUrl: './appointment-calendar.component.html',
  styleUrls: ['./appointment-calendar.component.scss']
})
export class AppointmentCalendarComponent implements OnInit {
  @ViewChild('calendar', { static: false })
  calendarComponent!: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    editable: true,
    selectable: true,
    events: [],       // sẽ gán động ở dưới
    dateClick: this.handleDateClick.bind(this),
    eventClick: this.handleEventClick.bind(this),
    eventContent: this.eventContent.bind(this),
  };

  constructor(
    private appointmentService: AppointmentCalendarService,
    private router: Router
  ) { }

  ngOnInit() {
    this.loadAppointments();
  }

  private loadAppointments() {
    this.appointmentService.getAllAppointments()
      .subscribe((list: Appointment[]) => {
        const events: EventInput[] = list.map(appt => ({
          id: appt.id.toString(),
          title: appt.doctorName ?? 'Đặng Văn Trường',    // hiển thị tên bác sĩ
          start: appt.appointmentDateTime,    // FullCalendar tự parse ISO string
          allDay: false
        }));

        // gán vào calendar và refetch
        this.calendarOptions = {
          ...this.calendarOptions,
          events
        };

        //   this.calendarComponent.getApi().refetchEvents();
        //   error: err => {
        // console.error('Không tải được lịch hẹn:', err);
        // }
      });
  }

  handleDateClick(info: any) {
    // ...
  }

  handleEventClick(info: any) {
    const ev = info.event;
    this.router.navigate(['/admin/appointment/viewAppointment'], {

      queryParams: {
        id: ev.id,
        title: ev.title,
        start: ev.start?.toISOString()
      }
    });

  }

  eventContent(info: any) {
    return { html: `<div class="fc-event-title">${info.event.title}</div>` };
  }
}
