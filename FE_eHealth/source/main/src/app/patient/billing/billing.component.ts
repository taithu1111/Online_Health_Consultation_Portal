// src/app/billing/billing.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, DatePipe, CurrencyPipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';

import { PaymentService } from '../billing/billing.servide';
import { PaymentDto } from '../billing/billing.model';

interface Billing {
  invoiceNo: string;
  doctorName: string;
  doctorEmail: string;
  date: Date;
  amount: number;
  tax: number;
  discount: number;
  total: number;
  transactionId: string;
}

@Component({
  selector: 'app-billing',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    CurrencyPipe,
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
  bills: Billing[] = [];
  displayedColumns = [
    'invoiceNo',
    'doctorName',
    'date',
    'amount',
    'tax',
    'discount',
    'total',
    'actions'
  ];

  private readonly TAX_RATE = 0.10;    // 10%
  private readonly DISCOUNT = 5;       // $5

  constructor(
    private route: ActivatedRoute,
    private paymentService: PaymentService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('appointmentId');
      if (id) {
        this.appointmentId = +id;
        this.loadBills();
      }
    });
  }

  private loadBills(): void {
    this.paymentService
      .getPaymentsByAppointmentId(this.appointmentId)
      .subscribe((payments: PaymentDto[]) => {
        this.bills = payments.map(p => {
          // Nếu DTO chưa có nested info, bạn cần bổ sung backend hoặc service
          const appt = (p as any).appointment;
          const doctor = appt?.doctor || {};
          const dateStr = appt?.date || (p as any).createdAt;

          const amount = p.amount;
          const taxAmt = parseFloat((amount * this.TAX_RATE).toFixed(2));
          const discountAmt = this.DISCOUNT;
          const totalAmt = parseFloat((amount + taxAmt - discountAmt).toFixed(2));

          return {
            invoiceNo: `A${p.id}`,
            doctorName: `${doctor.firstName || ''} ${doctor.lastName || ''}`.trim(),
            doctorEmail: doctor.email || '',
            date: new Date(dateStr),
            amount,
            tax: taxAmt,
            discount: discountAmt,
            total: totalAmt,
            transactionId: p.transactionId
          } as Billing;
        });
      });
  }

  download(bill: Billing): void {
    console.log('Download invoice for', bill.invoiceNo);
  }

  view(bill: Billing): void {
    console.log('View invoice', bill.invoiceNo);
  }

  print(bill: Billing): void {
    window.print();
  }
}
