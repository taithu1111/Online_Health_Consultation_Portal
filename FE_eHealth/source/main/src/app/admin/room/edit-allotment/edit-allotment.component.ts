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
    selector: 'app-edit-allotment',
    templateUrl: './edit-allotment.component.html',
    styleUrls: ['./edit-allotment.component.scss'],
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
export class EditAllotmentComponent {
  roomForm: UntypedFormGroup;
  formdata = {
    id: 'XYZ123', // Random ID for the room
    img: 'https://randomuser.me/api/portraits/men/32.jpg', // Random image URL
    patientName: 'John Doe', // Random patient name
    roomNo: '105', // Room number
    roomType: '2', // Room type (1: General, 2: Delux, etc.)
    gender: 'Male', // Patient gender
    admitDate: '2023-02-17T14:22:18Z', // Random admission date
    dischargeDate: '2023-02-19T14:22:18Z', // Random discharge date
    doctorAssigned: 'Dr. Smith', // Random doctor name
    status: 'Admitted', // Status of the patient (Admitted/Discharged)
    amountCharged: 2000.5, // Amount charged for the room
    age: 45, // Patient's age
    mobile: '9876543210', // Random mobile number (10 digits)
    email: 'john.doe@example.com', // Random email address
  };
  constructor(private fb: UntypedFormBuilder) {
    this.roomForm = this.createContactForm();
  }
  onSubmit() {
    console.log('Form Value', this.roomForm.value);
  }
  createContactForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.formdata.id, [Validators.required]], // Random ID for the room
      img: [this.formdata.img, [Validators.required]], // Image URL for the room
      patientName: [this.formdata.patientName, [Validators.required]], // Patient's name
      roomNo: [this.formdata.roomNo, [Validators.required]], // Room number
      roomType: [this.formdata.roomType, [Validators.required]], // Room type (General, Delux, etc.)
      gender: [this.formdata.gender, [Validators.required]], // Gender of the patient
      admitDate: [this.formdata.admitDate, [Validators.required]], // Admission date
      dischargeDate: [this.formdata.dischargeDate, [Validators.required]], // Discharge date
      doctorAssigned: [this.formdata.doctorAssigned, [Validators.required]], // Doctor assigned to the patient
      status: [this.formdata.status, [Validators.required]], // Patient status (Admitted/Discharged)
      amountCharged: [
        this.formdata.amountCharged,
        [Validators.required, Validators.min(0)],
      ], // Amount charged
      age: [this.formdata.age, [Validators.required, Validators.min(0)]], // Patient's age
      mobile: [
        this.formdata.mobile,
        [Validators.required, Validators.pattern('^[0-9]{10}$')],
      ], // Mobile number
      email: [this.formdata.email, [Validators.required, Validators.email]], // Email address
    });
  }
}
