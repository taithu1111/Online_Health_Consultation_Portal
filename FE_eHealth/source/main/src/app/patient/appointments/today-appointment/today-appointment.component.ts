import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { AppointmentService } from '../appointment-v1.service';
import { DatePipe, CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TodayDeleteComponent, DialogData } from './dialogs/delete/delete.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

interface UiAppointment {
  id: number;
  doctorName: string;
  appointmentDate: string;   // 'yyyy-MM-dd'
  appointmentTime: string;   // 'HH:mm:ss'
  status: string;
  type: string;
  notes: string;
  diagnosis: string;
}

@Component({
  selector: 'app-today-appointment',
  templateUrl: './today-appointment.component.html',
  styleUrls: ['./today-appointment.component.scss'],
  providers: [DatePipe],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    BreadcrumbComponent,
    MatDialogModule,
  ]
})
export class TodayAppointmentComponent implements OnInit {
  allAppointments: UiAppointment[] = [];
  filtered: UiAppointment[] = [];
  filterDate: Date | null = null;

  constructor(
    private svc: AppointmentService,
    private datePipe: DatePipe,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    const patientId = 7; // hoặc lấy dynamic từ context
    this.svc.getAppointmentsByPatientId(patientId).subscribe(arr => {
      // Map dữ liệu trả về về UiAppointment
      this.allAppointments = arr.map((item: any) => {
        const dt = new Date(item.appointmentDateTime);
        return {
          id: item.id,
          doctorName: item.doctorName,
          appointmentDate: this.datePipe.transform(dt, 'yyyy-MM-dd')!,
          appointmentTime: this.datePipe.transform(dt, 'HH:mm:ss')!,
          status: item.status ?? 'Pending',
          type: item.type,
          notes: item.notes ?? '',
          diagnosis: item.diagnosis ?? 'No Diagnosis'
        };
      });
      // Chạy filter mặc định là ngày hôm nay
      this.onFilterDate(new Date());
    });
  }

  onFilterDate(d: Date | null) {
    this.filterDate = d;
    if (!d) {
      // Clear filter → show all
      this.filtered = [...this.allAppointments];
    } else {
      const s = this.datePipe.transform(d, 'yyyy-MM-dd');
      this.filtered = this.allAppointments.filter(x => x.appointmentDate === s);
    }
  }

  openDeleteDialog(appt: UiAppointment) {
    const data: DialogData = {
      id: appt.id,
      doctorName: appt.doctorName,
      date: appt.appointmentDate,
      type: appt.type
    };

    const ref = this.dialog.open(TodayDeleteComponent, { data });

    ref.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        // remove khỏi cả allAppointments + filtered
        this.allAppointments = this.allAppointments.filter(a => a.id !== appt.id);
        this.filtered = this.filtered.filter(a => a.id !== appt.id);
      }
    });
  }
}
