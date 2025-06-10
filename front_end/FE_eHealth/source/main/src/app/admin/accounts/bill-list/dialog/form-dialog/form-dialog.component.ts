// src/app/admin/accounts/bill-list/dialog/form-dialog/form-dialog.component.ts

import {
  MAT_DIALOG_DATA,
  MatDialogRef
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { PaymentService } from '../../../payment.service';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
  ReactiveFormsModule,
  FormsModule
} from '@angular/forms';
import { BillList } from '../../../payment.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface DialogData {
  action: 'add' | 'edit';
  billList?: BillList;
}

@Component({
  selector: 'app-bill-list-form',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class FormDialogComponent {
  action: 'add' | 'edit';
  dialogTitle: string;
  billListForm: UntypedFormGroup;
  billList: BillList;

  constructor(
    public dialogRef: MatDialogRef<FormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: UntypedFormBuilder,
    private paymentService: PaymentService
  ) {
    this.action = data.action;
    this.billList = data.billList
      ? data.billList
      : new BillList({});

    this.dialogTitle =
      this.action === 'edit'
        ? `Edit Bill for ${this.billList.patientName}`
        : 'New Bill';

    this.billListForm = this.createBillListForm();
  }

  private createBillListForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.billList.id],
      patientName: [this.billList.patientName],
      doctorName: [this.billList.doctorName],
      date: [this.billList.date],
      tax: [this.billList.tax],
      discount: [this.billList.discount],
      initialAmount: [this.billList.initialAmount],
      total: [this.billList.total],
      status: [this.billList.status, [Validators.required]],
    });
  }

  get statusControl() {
    return this.billListForm.get('status')!;
  }
  get initialAmountControl() {
    return this.billListForm.get('initialAmount')!;
  }

  submit(): void {
    if (this.billListForm.invalid) {
      this.billListForm.markAllAsTouched();
      return;
    }

    if (this.action === 'edit') {
      const updatedStatus = this.billListForm.get('status')!.value as string;
      // const updatedAmount = this.billListForm.get('initialAmount')!.value as number;
      const paymentId = Number(this.billList.id);

      this.paymentService.updateStatus(paymentId, updatedStatus).subscribe({
        next: () => {
          // Khi server trả về OK, chỉ close dialog với { id, status }
          this.dialogRef.close({
            id: this.billList.id,         // giữ nguyên kiểu string
            status: updatedStatus,
            // initialAmount: updatedAmount
          });
        },
        error: (err) => {
          console.error('Update Error:', err);
        }
      });
    } else {
      // Xử lý add mới (nếu cần)
      // ...
      this.dialogRef.close();
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
