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
import { AppointmentService } from '../appointment-v1.service';
import { CalendarService } from '../../../calendar/calendar.service';
import { AvailableSlotDto } from '../../../calendar/calendar.model';
import { CommonModule } from '@angular/common';
import { distinctUntilChanged } from 'rxjs/operators';

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
    MatButtonModule,
  ],
  templateUrl: './book-appointment.component.html',
  styleUrls: ['./book-appointment.component.scss']
})
export class BookAppointmentComponent {
  bookingForm: UntypedFormGroup;
  availableSlots: AvailableSlotDto[] = []; // danh sách slot trống
  isSubmitting = false;

  constructor(
    private fb: UntypedFormBuilder,
    private appointmentService: AppointmentService,
    private calendarService: CalendarService   //  inject CalendarService
  ) {
    this.bookingForm = this.fb.group({
      patientId: [1, [Validators.required]],   // lấy từ auth/user context
      doctorId: ['', [Validators.required]],
      doa: ['', [Validators.required]],        // Date
      timeSlot: ['', [Validators.required]],   // "08:00:00"
      type: ['', [Validators.required]],       // "online" or "in-person"
      notes: [''],
      uploadFile: ['']
    });
  }

  get f() {
    return this.bookingForm.controls;
  }

  private loadSlots() {
    const docId = this.bookingForm.value.doctorId;
    const date = this.bookingForm.value.doa;
    if (!docId || !date) {
      this.availableSlots = [];
      return;
    }

    // chuyển Date thành "yyyy-MM-dd"
    const isoDate = (date as Date).toISOString().split('T')[0];
    this.calendarService.getAvailableSlots(docId, isoDate)
      .subscribe({
        next: (slots) => {
          this.availableSlots = slots.map(s => ({
            ...s,
            startTime: s.slotStart.slice(0, 5), // chỉ lấy giờ:phút
            endTime: s.slotEnd.slice(0, 5)
          }));
          // reset lại timeSlot mỗi lần load
          this.bookingForm.get('timeSlot')!.reset();
        },
        error: (err) => {
          console.error('Lỗi khi fetch slots', err);
          this.availableSlots = [];
        }
      });
  }
  ngOnInit() {
    // mỗi khi chọn bác sĩ hoặc chọn ngày, gọi API lấy slot
    this.bookingForm.get('doctorId')!.valueChanges
      .pipe(distinctUntilChanged())
      .subscribe({
        next: () => this.loadSlots()
      });

    this.bookingForm.get('doa')!.valueChanges
      .pipe(distinctUntilChanged())
      .subscribe({
        next: () => this.loadSlots()
      });
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
