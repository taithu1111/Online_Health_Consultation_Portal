import { Component } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';

@Component({
    selector: 'app-add-room',
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
    ],
    templateUrl: './add-room.component.html',
    styleUrl: './add-room.component.scss'
})
export class AddRoomComponent {
  roomForm: UntypedFormGroup;
  constructor(private fb: UntypedFormBuilder) {
    this.roomForm = this.fb.group({
      roomNumber: ['', [Validators.required]], // Room Number (e.g., 101, 102)
      departmentName: ['', [Validators.required]], // Department the room belongs to (e.g., ICU, Surgery)
      roomType: ['', [Validators.required]], // Type of room (e.g., Single, Double, ICU, Operating Room)
      floor: ['', [Validators.required]], // Floor number where the room is located
      bedCapacity: ['', [Validators.required, Validators.min(1)]], // Maximum number of beds the room can accommodate
      roomStatus: ['', [Validators.required]], // Current room status (e.g., Available, Occupied, Under Maintenance)
      roomCategory: ['', [Validators.required]], // Room category (e.g., Critical Care, Private, General)
      assignedStaff: ['', [Validators.required]], // Medical staff assigned to the room
      roomFeatures: ['', [Validators.required]], // Features and equipment available in the room (e.g., Ventilator, Oxygen Supply)
      roomRate: ['', [Validators.required, Validators.min(0)]], // Room rate (e.g., per day rate)
      specialInstructions: ['', []],
    });
  }
  onSubmit() {
    console.log('Form Value', this.roomForm.value);
  }
}
