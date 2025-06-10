import { Component, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';

import { PaymentService } from '../payment.service';
import { PaymentDto } from '../payment.model';
import { AppointmentService, AppointmentDto } from '../add-bill/get-appointment.service';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-add-bill',
  templateUrl: './add-bill.component.html',
  styleUrls: ['./add-bill.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatButtonModule,
    MatSnackBarModule,
    BreadcrumbComponent
  ]
})
export class AddBillComponent implements OnInit {
  billForm: UntypedFormGroup;
  isSubmitting = false;

  constructor(
    private fb: UntypedFormBuilder,
    private billListService: PaymentService,
    private appointmentService: AppointmentService,
    private snackBar: MatSnackBar
  ) {
    this.billForm = this.fb.group({
      appointmentId: [null, [Validators.required, Validators.min(1)]],
      patientName: [''],
      doctorName: [''],
      appointmentDate: [''],
      appointmentType: [''],
      amount: [null, [Validators.required, Validators.min(0)]],
      status: ['Pending', [Validators.required]]
    });
  }

  ngOnInit(): void { }
  onAppointmentIdBlur(): void {
    const idControl = this.billForm.get('appointmentId');
    if (!idControl) {
      return;
    }
    const rawValue = idControl.value;
    // Nếu rỗng hoặc không phải số, reset các ô chỉ đọc
    if (rawValue === null || rawValue === undefined || isNaN(rawValue)) {
      this.clearAppointmentInfo();
      return;
    }
    // Nếu nhập số < 1 thì cũng reset
    const idNum = Number(rawValue);
    if (idNum < 1) {
      this.clearAppointmentInfo();
      return;
    }
    // Gọi API khi user blur khỏi ô và giá trị hợp lệ
    this.fetchAppointmentAndPopulate(idNum);
  }

  private fetchAppointmentAndPopulate(id: number): void {
    this.appointmentService.getAppointmentById(id).subscribe({
      next: (app: AppointmentDto) => {
        this.billForm.patchValue({
          patientName: app.patientName,
          doctorName: app.doctorName,
          appointmentDate: this.formatDate(app.appointmentDateTime),
          appointmentType: app.type
        });
      },
      error: (err) => {
        console.error('Không lấy được appointment', err);
        this.clearAppointmentInfo();
        this.snackBar.open(
          `Không tìm thấy Appointment có ID = ${id}`,
          'Đóng',
          { duration: 3000 }
        );
      }
    });
  }

  private clearAppointmentInfo(): void {
    this.billForm.patchValue({
      patientName: '',
      doctorName: '',
      appointmentDate: '',
      appointmentType: ''
    });
  }

  private formatDate(isoStr: string): string {
    const d = new Date(isoStr);
    const dd = String(d.getDate()).padStart(2, '0');
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const yyyy = d.getFullYear();
    return `${dd}/${mm}/${yyyy}`;
  }

  onSubmit(): void {
    if (this.billForm.invalid) {
      this.billForm.markAllAsTouched();
      return;
    }
    this.isSubmitting = true;

    const raw = this.billForm.getRawValue() as {
      appointmentId: number;
      patientName: string;
      doctorName: string;
      appointmentDate: string;
      appointmentType: string;
      amount: number;
      status: string;
    };

    const payload: Partial<PaymentDto> = {
      appointmentId: raw.appointmentId,
      amount: raw.amount,
      status: raw.status
    };

    this.billListService.createBill(payload).subscribe({
      next: (created: PaymentDto) => {
        this.isSubmitting = false;
        this.snackBar.open(
          `Tạo payment thành công (ID: ${created.id}, Status: ${created.status})`,
          'Đóng',
          { duration: 3000 }
        );
        this.billForm.reset({
          appointmentId: null,
          patientName: '',
          doctorName: '',
          appointmentDate: '',
          appointmentType: '',
          amount: null,
          status: 'Pending'
        });
      },
      error: (err) => {
        console.error('Lỗi khi tạo payment:', err);
        this.isSubmitting = false;
        this.snackBar.open(
          'Có lỗi xảy ra khi tạo payment. Vui lòng thử lại.',
          'Đóng',
          { duration: 3000 }
        );
      }
    });
  }
}
