// src/app/billing/billing.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { forkJoin } from 'rxjs';

import { PaymentService } from '../billing/billing.servide';
import { PaymentDto } from '../billing/billing.model';
import { AppointmentService, } from '../appointments/appointment-v1.service';
import { AuthService } from '../../core/service/auth.service';
import { O } from '@angular/cdk/overlay-module.d-B3qEQtts';

interface BillingRow {
  invoiceNo: string;
  doctorName: string;
  doctorEmail?: string;
  date: Date;
  amount: number;
  taxPct: number;
  taxAmt: number;
  discountAmt: number;
  total: number;
  transactionId: string;
}

@Component({
  selector: 'app-billing',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    BreadcrumbComponent
  ],
  templateUrl: './billing.component.html',
  styleUrls: ['./billing.component.scss']
})
export class BillingComponent implements OnInit {
  patientId!: number;
  displayedColumns = [
    'invoiceNo', 'doctorName', 'date', 'amount', 'taxPct', 'discountAmt', 'total', 'actions'
  ];
  dataSource: BillingRow[] = [];

  private readonly TAX_RATE = 0.10;
  private readonly DISCOUNT_AMT = 5;

  constructor(
    private route: ActivatedRoute,
    private auth: AuthService,
    private paySvc: PaymentService,
    private apptSvc: AppointmentService
  ) { }

  ngOnInit(): void {
    const patient = this.auth.getCurrentUser();
    this.patientId = patient?.userId ?? 0;
    console.log('Loaded patientId from AuthService:', this.patientId);
    if (!this.patientId) {
      return;
    }
    this.loadAll();
  }

  private loadAll() {
    this.paySvc.getPaymentsByPatientId(this.patientId).subscribe({
      next: payments => {
        console.log('Payments:', payments); //kiểm tra xem payments đã được lấy chưa
        const appointmentCalls = payments.map(p =>
          this.apptSvc.getAppointmentById(p.appointmentId)
        );

        forkJoin(appointmentCalls).subscribe({
          next: appointments => {
            console.log('Appointments:', appointments); //kiểm tra xem appointments đã được lấy chưa
            this.dataSource = payments.map((p, idx) => {
              const appt = appointments[idx];
              const taxAmt = parseFloat((p.amount * this.TAX_RATE).toFixed(2));
              const discountAmt = this.DISCOUNT_AMT ?? 0;
              const total = parseFloat((p.amount + taxAmt - discountAmt).toFixed(2));

              return {
                invoiceNo: `P${p.id}`,
                doctorName: appt?.doctorName ?? 'N/A',
                doctorEmail: appt?.email ?? '',
                date: new Date(appt?.appointmentDateTime ?? new Date()),
                amount: p.amount,
                taxPct: this.TAX_RATE * 100,
                taxAmt,
                discountAmt,
                total,
                transactionId: p.transactionId
              };
            });
            console.log('DataSource:', this.dataSource); //kiểm tra xem dataSource đã được cập nhật chưa
          },
          error: err => console.error('Failed to load appointments', err)
        });
      },
      error: err => console.error('Failed to load payments', err)
    });
  }

  download(row: BillingRow) { /* TODO */ }
  view(row: BillingRow) { /* TODO */ }
  print(row: BillingRow) { window.print(); }
}
