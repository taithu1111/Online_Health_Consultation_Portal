// import { Component } from '@angular/core';
// import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { MatButtonModule } from '@angular/material/button';
// import { FileUploadComponent } from '@shared/components/file-upload/file-upload.component';
// import { MatButtonToggleModule } from '@angular/material/button-toggle';
// import { MatDatepickerModule } from '@angular/material/datepicker';
// import { MatOptionModule } from '@angular/material/core';
// import { MatSelectModule } from '@angular/material/select';
// import { MatInputModule } from '@angular/material/input';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
// @Component({
//     selector: 'app-book-appointment',
//     templateUrl: './book-appointment.component.html',
//     styleUrls: ['./book-appointment.component.scss'],
//     imports: [
//         BreadcrumbComponent,
//         FormsModule,
//         ReactiveFormsModule,
//         MatFormFieldModule,
//         MatInputModule,
//         MatSelectModule,
//         MatOptionModule,
//         MatDatepickerModule,
//         MatButtonToggleModule,
//         FileUploadComponent,
//         MatButtonModule,
//     ]
// })
// export class BookAppointmentComponent {
//   bookingForm: UntypedFormGroup;
//   hide3 = true;
//   agree3 = false;
//   isDisabled = true;
//   constructor(private fb: UntypedFormBuilder) {
//     this.bookingForm = this.fb.group({
//       first: ['', [Validators.required, Validators.pattern('[a-zA-Z]+')]],
//       last: [''],
//       gender: ['', [Validators.required]],
//       mobile: ['', [Validators.required]],
//       address: [''],
//       email: [
//         '',
//         [Validators.required, Validators.email, Validators.minLength(5)],
//       ],
//       dob: ['', [Validators.required]],
//       doctor: ['', [Validators.required]],
//       doa: ['', [Validators.required]],
//       timeSlot: ['', [Validators.required]],
//       injury: [''],
//       note: [''],
//       uploadFile: [''],
//     });
//   }
//   onSubmit() {
//     console.log('Form Value', this.bookingForm.value);
//   }

//   get f() {
//     return this.bookingForm.controls;
//   }
// }


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
import { AppointmentService } from '../appointment-v1.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [
    BreadcrumbComponent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    MatButtonToggleModule,
    FileUploadComponent,
    MatButtonModule,
  ],
  templateUrl: './book-appointment.component.html',
  styleUrls: ['./book-appointment.component.scss']
})
export class BookAppointmentComponent {
  bookingForm: UntypedFormGroup;
  isSubmitting = false;

  constructor(
    private fb: UntypedFormBuilder,
    private appointmentService: AppointmentService
  ) {
    this.bookingForm = this.fb.group({
      patientId: [7, [Validators.required]],   // bạn sẽ lấy từ auth/user context
      doctorId: ['', [Validators.required]],
      doa: ['', [Validators.required]],        // Date
      timeSlot: ['', [Validators.required]],   // e.g. "08:00:00"
      type: ['', [Validators.required]],       // e.g. "video" or "in-person"
      notes: [''],
      uploadFile: ['']
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
