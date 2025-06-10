import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UntypedFormBuilder, UntypedFormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Appointments } from '../../appointments.model';

export interface DialogData {
  appointments: Appointments;
  action: string;
}

@Component({
  selector: 'app-doctor-appointment-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss'],
})
export class DoctorAppointmentFormComponent {
  appointmentsForm: UntypedFormGroup;

  constructor(
    public dialogRef: MatDialogRef<DoctorAppointmentFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: UntypedFormBuilder
  ) {
    // Chỉ khởi tạo control cho status
    this.appointmentsForm = this.fb.group({
      status: [data.appointments.status, Validators.required]
    });
  }

  /**
   * Lưu và đóng dialog, trả về instance mới của Appointments
   */
  onSave(): void {
    if (this.appointmentsForm.valid) {
      // Tạo đối tượng Appointments mới để bảo đảm đầy đủ properties
      const updated = new Appointments({
        ...this.data.appointments,
        status: this.appointmentsForm.value.status
      });
      this.dialogRef.close(updated);
    }
  }

  /**
   * Đóng dialog mà không thay đổi gì
   */
  onCancel(): void {
    this.dialogRef.close();
  }
}
