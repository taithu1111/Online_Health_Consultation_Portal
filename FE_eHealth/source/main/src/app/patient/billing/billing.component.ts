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
  appointmentId!: number;
  displayedColumns = [
    'invoiceNo', 'doctorName', 'date', 'amount', 'taxPct', 'discountAmt', 'total', 'actions'
  ];
  dataSource: BillingRow[] = [];

  private readonly TAX_RATE = 0.10;
  private readonly DISCOUNT_AMT = 5;

  constructor(
    private route: ActivatedRoute,
    private paySvc: PaymentService,
    private apptSvc: AppointmentService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(mp => {
      const raw = mp.get('appointmentId');
      if (!raw) return;
      this.appointmentId = +raw;
      this.loadAll();
    });
  }

  private loadAll() {
    forkJoin({
      appt: this.apptSvc.getAppointmentById(this.appointmentId),
      payments: this.paySvc.getPaymentsByAppointmentId(this.appointmentId)
    }).subscribe(({ appt, payments }) => {
      this.dataSource = payments.map(p => {
        const taxAmt = parseFloat((p.amount * this.TAX_RATE).toFixed(2));
        const discountAmt = this.DISCOUNT_AMT;
        const total = parseFloat((p.amount + taxAmt - discountAmt).toFixed(2));
        return {
          invoiceNo: `A${p.id}`,
          doctorName: appt.doctorName,
          doctorEmail: appt.email ?? undefined,
          date: new Date(appt.appointmentDateTime),
          amount: p.amount,
          taxPct: this.TAX_RATE * 100,
          taxAmt,
          discountAmt,
          total,
          transactionId: p.transactionId
        };
      });
    }, err => {
      console.error('Load billing data failed', err);
    });
  }

  download(row: BillingRow) { /* TODO */ }
  view(row: BillingRow) { /* TODO */ }
  print(row: BillingRow) { window.print(); }
}
