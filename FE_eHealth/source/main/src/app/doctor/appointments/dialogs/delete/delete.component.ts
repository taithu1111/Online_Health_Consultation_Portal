import { Component, Inject } from '@angular/core';
import {
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppointmentService } from '../../appointments.service';

export interface DialogData {
  id: number;
  patientName: string;
  appointmentDate: string;
  appointmentTime: string;
  status: string;
}

@Component({
  selector: 'app-upcoming-appointment-delete',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatButtonModule,
    MatDialogClose
  ],
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.scss']
})
export class AppointmentDelete {
  constructor(
    private dialogRef: MatDialogRef<AppointmentDelete>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private appointmentService: AppointmentService
  ) { }

  confirmDelete(): void {
    // gọi thẳng vào API deleteAppointment trong appointment-v1.service.ts
    this.appointmentService.deleteAppointment(this.data.id).subscribe({
      next: () => {
        // truyền về true để parent biết đã xóa thành công
        this.dialogRef.close(true);
      },
      error: err => {
        console.error('Delete Error:', err);
        // bạn có thể show toast hoặc snackbar báo lỗi ở đây
      }
    });
  }

  cancel(): void {
    // đóng dialog mà không truyền gì (hoặc truyền false)
    this.dialogRef.close(false);
  }
}
