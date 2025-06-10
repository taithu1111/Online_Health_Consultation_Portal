// src/app/admin/accounts/bill-list/dialog/delete/delete.component.ts

import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { PaymentService } from '../../../payment.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { BillList } from '../../../payment.model';

// Import các module của Material mà template đang sử dụng
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface BillListDeleteData {
  id: string;
  patientName: string;
  doctorName: string;
  total: string;
}

@Component({
  selector: 'app-bill-list-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatSnackBarModule,
    MatListModule,
    MatDividerModule,
    MatButtonModule
  ]
})
export class BillListDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<BillListDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: BillListDeleteData,
    private paymentService: PaymentService,
    private snackBar: MatSnackBar
  ) { }

  confirmDelete(): void {
    const paymentId = Number(this.data.id);
    this.paymentService.deleteBill(paymentId).subscribe({
      next: () => {
        this.snackBar.open('Delete Record Successfully!', '', {
          duration: 2000,
          panelClass: 'snackbar-success'
        });
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Delete failed:', error);
        this.snackBar.open('Failed to delete record. Please try again.', '', {
          duration: 3000,
          panelClass: 'snackbar-danger'
        });
        this.dialogRef.close(false);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
