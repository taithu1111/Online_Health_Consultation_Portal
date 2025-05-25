/* eslint-disable @typescript-eslint/no-empty-function */
import { Component, OnInit, ViewChild } from '@angular/core';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexYAxis,
  ApexPlotOptions,
  ApexStroke,
  ApexLegend,
  ApexNonAxisChartSeries,
  ApexMarkers,
  ApexGrid,
  ApexTitleSubtitle,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatCardModule } from '@angular/material/card';
import { NgScrollbar } from 'ngx-scrollbar';
import { MedicineListComponent } from '@shared/components/medicine-list/medicine-list.component';
import { ReportListComponent } from '@shared/components/report-list/report-list.component';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { UpcomingAppointmentComponent } from '../appointments/upcoming-appointment/upcoming-appointment.component';
import { AppointmentService } from '../appointments/appointment-v1.service';
import { DatePipe } from '@angular/common';
import { AuthService } from '../../core/service/auth.service';
import { PaymentService } from '../billing/billing.servide';
import { forkJoin } from 'rxjs';

export type areaChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
  legend: ApexLegend;
  colors: string[];
};

export type restRateChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  colors: string[];
  yaxis: ApexYAxis;
  grid: ApexGrid;
  tooltip: ApexTooltip;
  legend: ApexLegend;
  title: ApexTitleSubtitle;
};
export type performanceRateChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  colors: string[];
  yaxis: ApexYAxis;
  grid: ApexGrid;
  tooltip: ApexTooltip;
  legend: ApexLegend;
  title: ApexTitleSubtitle;
};

export interface Medicine {
  name: string;
  icon: string;
  dosage: string;
}

export type radialChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  colors: string[];
  plotOptions: ApexPlotOptions;
};
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  imports: [
    BreadcrumbComponent,
    NgApexchartsModule,
    MatButtonModule,
    MatTabsModule,
    MatIconModule,
    MatCardModule,
    NgScrollbar,
    MedicineListComponent,
    ReportListComponent,
    CommonModule,
    MatTableModule,
    MatButtonToggleModule
  ],
  providers: [DatePipe]
})
export class DashboardComponent implements OnInit {
  @ViewChild('chart')
  chart!: ChartComponent;
  public areaChartOptions!: Partial<areaChartOptions>;
  public radialChartOptions!: Partial<radialChartOptions>;
  public restRateChartOptions!: Partial<restRateChartOptions>;
  public performanceRateChartOptions!: Partial<performanceRateChartOptions>;
  upcomingAppointments: any[] = [];
  billingData: { invoiceNo: string; date: Date; total: number }[] = [];
  constructor(
    private appointmentService: AppointmentService,
    private datePipe: DatePipe,
    private authService: AuthService,
    private paymentService: PaymentService
  ) { }
  ngOnInit() {
    this.loadUpcomingAppointments();
    this.loadBilling();
  }

  loadUpcomingAppointments() {
    // const patientId = Number(this.authService.getDecodedToken()?.nameid || 0);
    const patient = this.authService.getCurrentUser();
    const patientId = patient?.userId ?? 0;
    if (!patientId) {
      this.upcomingAppointments = [];
      return;
    }
    this.appointmentService.getAppointmentsByPatientId(patientId).subscribe({
      next: (apiData) => {
        this.upcomingAppointments = apiData.map((item: any) => {
          const dt = new Date(item.appointmentDateTime);
          return {
            id: item.id,
            doctorName: item.doctorName,
            appointmentDate: this.datePipe.transform(dt, 'dd MMM yyyy'),
            appointmentTime: this.datePipe.transform(dt, 'HH:mm'),
            status: item.status ?? 'Pending',
            type: item.type,
            notes: item.notes,
            diagnosis: item.diagnosis ?? 'No Diagnosis',
            contactNumber: item.doctor?.contactNumber ?? 'N/A',
          };
        });
      },
      error: (err) => {
        console.error('Failed to load appointments:', err);
        this.upcomingAppointments = [];
      },
    });
  }

  loadBilling() {
    const patientId = this.authService.getCurrentUser()?.userId ?? 0;
    if (!patientId) return;

    this.paymentService.getPaymentsByPatientId(patientId).subscribe({
      next: payments => {
        const appointmentCalls = payments.map(p =>
          this.appointmentService.getAppointmentById(p.appointmentId)
        );

        forkJoin(appointmentCalls).subscribe(appointments => {
          this.billingData = payments.map((p, idx) => {
            const appt = appointments[idx];
            const tax = p.amount * 0.1;
            const discount = 5;
            const total = +(p.amount + tax - discount).toFixed(2);

            return {
              invoiceNo: `P${p.id}`,
              date: new Date(appt?.appointmentDateTime ?? new Date()),
              total
            };
          });
        });
      },
      error: err => {
        console.error('Failed to load billing data', err);
      }
    });
  }

}
