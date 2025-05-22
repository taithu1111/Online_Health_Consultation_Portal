import { Route } from '@angular/router';
import { BookAppointmentComponent } from './bookappointment/bookappointment.component';
import { ViewappointmentComponent } from './viewappointment/viewappointment.component';
import { Page404Component } from '../../authentication/page404/page404.component';
import { AppointmentCalendarComponent } from './appointment-calendar/appointment-calendar.component';
export const APPOINTMENT_ROUTE: Route[] = [
  {
    path: 'appointment-calendar',
    component: AppointmentCalendarComponent,
  },
  {
    path: 'bookAppointment',
    component: BookAppointmentComponent,
  },
  {
    path: 'viewAppointment',
    component: ViewappointmentComponent,
  },
  { path: '**', component: Page404Component },
];
