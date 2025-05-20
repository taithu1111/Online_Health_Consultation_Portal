import { MAT_DIALOG_DATA, MatDialogRef, MatDialogContent, MatDialogClose, } from '@angular/material/dialog';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { AppointmentService } from '../../appointment.service';
import { UntypedFormControl, Validators, UntypedFormGroup, UntypedFormBuilder, FormsModule, ReactiveFormsModule, } from '@angular/forms';
import { Appointment } from '../../appointment.model';
import { MAT_DATE_LOCALE, provideNativeDateAdapter, } from '@angular/material/core';
import { formatDate, CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTimepickerModule } from '@angular/material/timepicker';

export interface DialogData {
  id: number;
  action: string;
  appointment: Appointment;
}

@Component({
  selector: 'app-view-appointment-form',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  providers: [
    provideNativeDateAdapter(),
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatDialogContent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatDatepickerModule,
    MatSelectModule,
    MatDialogClose,
    MatTimepickerModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewAppointmentFormComponent {
  action: string;
  dialogTitle: string;
  appointmentForm: UntypedFormGroup;
  appointment: Appointment;

  constructor(
    public dialogRef: MatDialogRef<ViewAppointmentFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public appointmentService: AppointmentService,
    private fb: UntypedFormBuilder
  ) {
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.appointment.patientName : 'New Appointment';
    this.appointment =
      this.action === 'edit' ? data.appointment : new Appointment({});
    this.appointmentForm = this.createAppointmentForm();
  }

  createAppointmentForm(): UntypedFormGroup {
    const timeValue = this.appointment.appointmentDateTime
      ? this.parseTimeStringFromDate(this.appointment.appointmentDateTime)
      : null;
    const dateValue = this.appointment.appointmentDateTime
      ? new Date(this.appointment.appointmentDateTime)
      : null;

    return this.fb.group({
      id: [this.appointment.id],
      patientName: [this.appointment.patientName, [Validators.required]],
      email: [this.appointment.email, [Validators.required, Validators.email]],
      gender: [this.appointment.gender, [Validators.required]],
      date: [dateValue, [Validators.required]],
      time: [timeValue, [Validators.required]],
      phone: [this.appointment.phone, [Validators.required]], // Đồng bộ với tên phone
      doctorName: [this.appointment.doctorName, [Validators.required]],
      diagnosis: [this.appointment.diagnosis],
      status: [this.appointment.status],
      type: [this.appointment.type],
      notes: [this.appointment.notes],
    });
  }

  private parseTimeStringFromDate(dateTime: string): Date | null {
    if (!dateTime) return null;
    const dateObj = new Date(dateTime);
    return dateObj;
  }

  getErrorMessage(control: UntypedFormControl): string {
    if (control.hasError('required')) {
      return 'This field is required';
    } else if (control.hasError('email')) {
      return 'Please enter a valid email';
    }
    return '';
  }

  submit() {
    if (this.appointmentForm.valid) {
      const formData = this.appointmentForm.getRawValue();

      const date: Date = formData.date;
      const time: Date = formData.time;

      const appointmentDateTime = new Date(date);
      appointmentDateTime.setHours(time.getHours());
      appointmentDateTime.setMinutes(time.getMinutes());
      appointmentDateTime.setSeconds(0);
      appointmentDateTime.setMilliseconds(0);

      formData.appointmentDateTime = appointmentDateTime.toISOString();

      delete formData.date;
      delete formData.time;

      if (this.action === 'edit') {
        this.appointmentService.updateAppointment(formData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            alert('Update failed, please try again later!');
          },
        });
      } else {
        this.appointmentService.addAppointment(formData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
            alert('Add failed, please try again later!');
          },
        });
      }
    }
  }

  onNoClick(): void {
    this.appointmentForm.reset();
    this.dialogRef.close();
  }
}
