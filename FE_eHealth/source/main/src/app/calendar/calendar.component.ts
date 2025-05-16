import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import interactionPlugin from '@fullcalendar/interaction';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule, MatCheckboxChange } from '@angular/material/checkbox';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { CalendarService } from './calendar.service';
import { ScheduleDto, AvailableSlotDto, CreateScheduleCommand, UpdateScheduleCommand } from './calendar.model';
import { FormDialogComponent } from './dialogs/form-dialog/form-dialog.component';
import { DateSelectArg, EventClickArg, EventInput, CalendarOptions } from '@fullcalendar/core';
import { BreadcrumbComponent } from '../shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-calendar',
  standalone: true,
  imports: [
    CommonModule,
    FullCalendarModule,
    MatCardModule,
    MatButtonModule,
    MatCheckboxModule,
    MatDialogModule,
    FormDialogComponent,
    BreadcrumbComponent
  ],
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  calendarOptions: CalendarOptions = {
    plugins: [
      dayGridPlugin,
      timeGridPlugin,
      listPlugin,
      interactionPlugin
    ],
    initialView: 'dayGridMonth',
    selectable: true,
    editable: true,
    select: this.handleDateSelect.bind(this),
    eventClick: this.handleEventClick.bind(this),
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
    },
    events: []
  };

  filters = [
    { value: 'Available', checked: true },
    { value: 'Unavailable', checked: true }
  ];
  private rawSchedules: ScheduleDto[] = [];

  constructor(
    private calendarService: CalendarService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.loadSchedules(this.getCurrentDoctorId());
  }

  private loadSchedules(doctorId: number) {
    this.calendarService.getDoctorSchedules(doctorId)
      .subscribe(s => {
        this.rawSchedules = s;
        this.renderEvents();
      });
  }

  private renderEvents() {
    this.calendarOptions.events = this.rawSchedules
      .filter(s => this.filterEvent(s))
      .map(s => ({
        id: s.id.toString(),
        title: `${s.startTime.slice(0, 5)} - ${s.endTime.slice(0, 5)}`,
        daysOfWeek: [s.dayOfWeek],
        startTime: s.startTime,
        endTime: s.endTime,
        backgroundColor: s.isAvailable ? undefined : 'lightgray'
      } as EventInput));
  }

  private filterEvent(s: ScheduleDto) {
    return (s.isAvailable && this.filters[0].checked)
      || (!s.isAvailable && this.filters[1].checked);
  }

  changeCategory(e: MatCheckboxChange, f: { value: string, checked: boolean }) {
    f.checked = e.checked;
    this.renderEvents();
  }
  trackByFilter(i: number, f: any) { return f.value; }

  addNewEvent() {
    const doctorId = this.getCurrentDoctorId();
    const dialogRef = this.dialog.open(FormDialogComponent, {
      data: { date: new Date(), slots: [] as AvailableSlotDto[] }
    });
    dialogRef.afterClosed().subscribe((res: any) => {
      if (!res) return;
      const cmd: CreateScheduleCommand = {
        doctorId,
        dayOfWeek: (res.date as Date).getDay(),
        startTime: res.startTime + ':00',
        endTime: res.endTime + ':00',
        location: res.location,
        description: res.description
      };
      this.calendarService.createSchedule(cmd)
        .subscribe(() => this.loadSchedules(doctorId));
    });
  }

  private handleDateSelect(selectInfo: DateSelectArg) {
    const doctorId = this.getCurrentDoctorId();
    const date = new Date(selectInfo.start);
    const isoDate = date.toISOString().split('T')[0];
    this.calendarService.getAvailableSlots(doctorId, isoDate)
      .subscribe(slots => {
        const dialogRef = this.dialog.open(FormDialogComponent, {
          data: { date, slots }
        });
        dialogRef.afterClosed().subscribe((res: any) => {
          if (!res) return;
          const cmd: CreateScheduleCommand = {
            doctorId,
            dayOfWeek: date.getDay(),
            startTime: res.startTime + ':00',
            endTime: res.endTime + ':00',
            location: res.location,
            description: res.description
          };
          this.calendarService.createSchedule(cmd)
            .subscribe(() => this.loadSchedules(doctorId));
        });
      });
  }

  private handleEventClick(clickInfo: EventClickArg) {
    const id = +clickInfo.event.id;
    const date = clickInfo.event.start as Date;
    const doctorId = this.getCurrentDoctorId();
    const dialogRef = this.dialog.open(FormDialogComponent, {
      data: { scheduleId: id }
    });
    dialogRef.afterClosed().subscribe((res: any) => {
      if (!res) return;
      if (res.delete) {
        this.calendarService.deleteSchedule(id)
          .subscribe(() => this.loadSchedules(doctorId));
      } else {
        const cmd: UpdateScheduleCommand = {
          id,
          dayOfWeek: date.getDay(),
          startTime: res.startTime + ':00',
          endTime: res.endTime + ':00',
          location: res.location,
          description: res.description
        };
        this.calendarService.updateSchedule(id, cmd)
          .subscribe(() => this.loadSchedules(doctorId));
      }
    });
  }

  private getCurrentDoctorId(): number {
    return 1;  // TODO: lấy từ auth/route
  }
}
