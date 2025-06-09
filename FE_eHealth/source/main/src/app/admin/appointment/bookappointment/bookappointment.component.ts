import { Component } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { FileUploadComponent } from '@shared/components/file-upload/file-upload.component';
import { AppointmentService } from './appointment.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [
    CommonModule,
    BreadcrumbComponent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    MatButtonToggleModule,
    // FileUploadComponent,
    MatButtonModule,
  ],
  templateUrl: './bookappointment.component.html',
  styleUrls: ['./bookappointment.component.scss']
})
export class BookAppointmentComponent {
  bookingForm: UntypedFormGroup;
  isSubmitting = false;
  isLoadingDoctors = true;

  constructor(
    private fb: UntypedFormBuilder,
    private appointmentService: AppointmentService
  ) {
    this.bookingForm = this.fb.group({
      patientId: [, [Validators.required]],   // lấy từ auth/user context
      doctorId: ['', [Validators.required]],
      doa: ['', [Validators.required]],        // Date
      timeSlot: ['', [Validators.required]],   // "08:00:00"
      type: ['', [Validators.required]],       // "online" or "in-person"
      notes: [''],
      // uploadFile: ['']
    });
  }

  get f() {
    return this.bookingForm.controls;
  }

  onSubmit() {
    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      return;
    }
    this.isSubmitting = true;

    // Lấy thẳng các giá trị từ form.value
    const {
      patientId,
      doctorId,
      doa: rawDate,
      timeSlot,
      type,
      notes
    } = this.bookingForm.value;

    // Kết hợp date + time thành 1 Date object
    const date = new Date(rawDate);
    const [h, m, s] = timeSlot.split(':');
    date.setHours(+h, +m, +s);

    const payload = {
      patientId,
      doctorId,
      appointmentDateTime: date.toISOString(),
      type,
      notes
    };

    this.appointmentService.createAppointment(payload).subscribe({
      next: id => {
        console.log('Created appointment id=', id);
        alert(`Appointment booked! ID = ${id}`);
        this.bookingForm.reset({ patientId: payload.patientId });
      },
      error: err => {
        console.error(err);
        alert('Booking failed. Please try again.');
      },
      complete: () => {
        this.isSubmitting = false;
      }
    });
  }
}
