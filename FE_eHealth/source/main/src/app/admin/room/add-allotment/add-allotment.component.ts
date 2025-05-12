import { Component } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
@Component({
    selector: 'app-add-allotment',
    templateUrl: './add-allotment.component.html',
    styleUrls: ['./add-allotment.component.scss'],
    imports: [
        BreadcrumbComponent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatOptionModule,
        MatDatepickerModule,
        MatButtonModule,
    ]
})
export class AddAllotmentComponent {
  roomForm: UntypedFormGroup;
  constructor(private fb: UntypedFormBuilder) {
    this.roomForm = this.fb.group({
      id: ['', [Validators.required]], // Unique ID for the room
      img: ['', [Validators.required]], // Room image or thumbnail
      patientName: ['', [Validators.required]], // Patient's full name
      roomNo: ['', [Validators.required]], // Room number
      roomType: ['', [Validators.required]], // Type of room (e.g., private, shared)
      gender: ['', [Validators.required]], // Gender of the patient
      admitDate: ['', [Validators.required]], // Admission date
      dischargeDate: ['', [Validators.required]], // Discharge date
      doctorAssigned: ['', [Validators.required]], // Name of the doctor assigned to the patient
      status: ['', [Validators.required]], // Current status (e.g., admitted, discharged)
      amountCharged: ['', [Validators.required, Validators.min(0)]], // Amount charged
      age: ['', [Validators.required, Validators.min(0)]], // Patient age
      mobile: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]], // Mobile number validation
      email: ['', [Validators.required, Validators.email]], // Email validation
    });
  }
  onSubmit() {
    console.log('Form Value', this.roomForm.value);
  }
}
