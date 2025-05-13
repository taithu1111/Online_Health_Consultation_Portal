import { Component, Inject } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogClose
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule, formatDate } from '@angular/common';
import { AppointmentService } from '../../../appointment-v1.service';

interface DialogData {
  action: 'add' | 'edit';
  upcomingAppointment?: any;
}

@Component({
  selector: 'app-upcoming-appointment-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogClose,
  ],
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss']
})
export class FormDialogComponent {
  appointmentForm: UntypedFormGroup;
  dialogTitle: string;

  constructor(
    private fb: UntypedFormBuilder,
    private dialogRef: MatDialogRef<FormDialogComponent>,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private apptSvc: AppointmentService
  ) {
    this.dialogTitle =
      this.data.action === 'edit' ? 'Edit Appointment' : 'New Appointment';

    const appt = this.data.upcomingAppointment;
    const initialDate: Date | '' = appt?.date
      ? new Date(appt.date)
      : (appt?.appointmentDateTime ? new Date(appt.appointmentDateTime) : '');
    const initialTime: string = appt?.time
      ? appt.time
      : (appt?.appointmentDateTime
        ? appt.appointmentDateTime.split('T')[1].substr(0, 8)
        : '');

    this.appointmentForm = this.fb.group({
      patientId: [appt?.patientId ?? 7, Validators.required],
      doctorId: [appt?.doctorId ?? '', Validators.required],
      doa: [initialDate, Validators.required],
      timeSlot: [initialTime, Validators.required],
      type: [appt?.type ?? '', Validators.required],
      status: [appt?.status ?? 'awaiting', Validators.required],
      notes: [appt?.notes ?? ''],
      diagnosis: [appt?.diagnosis ?? 'no diagnosis', Validators.required],
    });
  }

  get f() {
    return this.appointmentForm.controls;
  }

  submit() {
    if (this.appointmentForm.invalid) {
      this.appointmentForm.markAllAsTouched();
      return;
    }

    const raw = this.appointmentForm.value;
    const date = raw.doa instanceof Date ? raw.doa : new Date(raw.doa);
    const [h, m, s] = raw.timeSlot.split(':').map((x: string) => +x);
    date.setHours(h, m, s, 0);

    const localDateTime = formatDate(
      date,
      "yyyy-MM-dd'T'HH:mm:ss",
      'en-US'
    );

    const dto: any = {
      patientId: raw.patientId,
      doctorId: raw.doctorId,
      appointmentDateTime: localDateTime,
      type: raw.type,
      status: raw.status,
      notes: raw.notes,
      diagnosis: raw.diagnosis,
    };

    if (this.data.action === 'edit') {
      const id = this.data.upcomingAppointment?.id;
      if (!id) {
        console.error('Cannot update: missing appointment id');
        return;
      }
      this.apptSvc.updateAppointment(id, dto, { responseType: 'text' }).subscribe({
        next: () => {
          this.snackBar.open('Cập nhật thành công', 'Đóng', { duration: 2000 });
          this.dialogRef.close(true);
        },
        error: err => {
          this.snackBar.open('Cập nhật thất bại', 'Đóng', { duration: 2000 });
          console.error(err);
        },
      });
    } else {
      this.apptSvc.createAppointment(dto).subscribe({
        next: () => {
          this.snackBar.open('Tạo mới thành công', 'Đóng', { duration: 2000 });
          this.dialogRef.close(true);
        },
        error: err => {
          this.snackBar.open('Tạo mới thất bại', 'Đóng', { duration: 2000 });
          console.error(err);
        },
      });
    }
  }

  cancel() {
    this.dialogRef.close(false);
  }
}
