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
  ApexFill,
  ApexGrid,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { NgScrollbar } from 'ngx-scrollbar';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { TodaysAppointmentComponent } from '@shared/components/todays-appointment/todays-appointment.component';
import { PatientGroupListComponent } from '@shared/components/patient-group-list/patient-group-list.component';
import { TodoWidgetComponent } from '@shared/components/todo-widget/todo-widget.component';
import { DocWelcomeCardComponent } from '@shared/components/doc-welcome-card/doc-welcome-card.component';
import { EmpStatusComponent } from '@shared/components/emp-status/emp-status.component';
export type areaChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  grid: ApexGrid;
  dataLabels: ApexDataLabels;
  legend: ApexLegend;
  colors: string[];
};

export type linechartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  grid: ApexGrid;
  fill: ApexFill;
  tooltip: ApexTooltip;
  stroke: ApexStroke;
  legend: ApexLegend;
  colors: string[];
};

export type radialChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  colors: string[];
  plotOptions: ApexPlotOptions;
};

interface Todo {
  title: string;
  done: boolean;
  priority: 'Low' | 'Normal' | 'High';
}
@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    imports: [
        BreadcrumbComponent,
        NgApexchartsModule,
        MatButtonModule,
        MatCardModule,
        MatMenuModule,
        NgScrollbar,
        MatIconModule,
        MatCheckboxModule,
        MatTooltipModule,
        TodaysAppointmentComponent,
        PatientGroupListComponent,
        TodoWidgetComponent,
        DocWelcomeCardComponent,
        EmpStatusComponent,
    ]
})
export class DashboardComponent implements OnInit {
  @ViewChild('chart')
  chart!: ChartComponent;
  public areaChartOptions!: Partial<areaChartOptions>;
  public radialChartOptions!: Partial<radialChartOptions>;
  public linechartOptions!: Partial<linechartOptions>;
  constructor() {}

  patientGroups = [
    {
      label: 'C',
      title: 'Cholesterol',
      patientCount: 5,
      colorClass: 'bg-orange',
    },
    {
      label: 'D',
      title: 'Diabetic',
      patientCount: 14,
      colorClass: 'bg-purple',
    },
    {
      label: 'L',
      title: 'Low Blood Pressure',
      patientCount: 10,
      colorClass: 'bg-green',
    },
    {
      label: 'H',
      title: 'Hypertension',
      patientCount: 21,
      colorClass: 'bg-cyan',
    },
    { label: 'M', title: 'Malaria', patientCount: 11, colorClass: 'bg-indigo' },
    {
      label: 'D',
      title: 'Dental Problem',
      patientCount: 17,
      colorClass: 'bg-brown',
    },
    {
      label: 'A',
      title: 'Asthma',
      patientCount: 8,
      colorClass: 'bg-yellow',
    },
    {
      label: 'R',
      title: 'Rheumatoid Arthritis',
      patientCount: 9,
      colorClass: 'bg-red',
    },
    {
      label: 'S',
      title: 'Stroke',
      patientCount: 6,
      colorClass: 'bg-blue',
    },
  ];

  // TODO start
  tasks: Todo[] = [
    { title: 'Review patient charts', done: false, priority: 'High' },
    { title: 'Complete patient prescriptions', done: false, priority: 'High' },
    {
      title: 'Follow-up with patients for test results',
      done: false,
      priority: 'Normal',
    },
    {
      title: 'Consult with specialists on patient cases',
      done: false,
      priority: 'High',
    },
    { title: 'Organize medical supplies', done: false, priority: 'Low' },
    {
      title: 'Check and update patient schedules',
      done: false,
      priority: 'High',
    },
    {
      title: 'Prepare for medical conference',
      done: false,
      priority: 'Normal',
    },
    {
      title: 'Answer patient queries via email or phone',
      done: false,
      priority: 'Normal',
    },
    { title: 'Attend medical staff meeting', done: false, priority: 'High' },
    {
      title: 'Update medical records for patients',
      done: false,
      priority: 'High',
    },
    {
      title: 'Plan continuing medical education (CME)',
      done: false,
      priority: 'Low',
    },
    { title: 'Review latest medical research', done: false, priority: 'Low' },
    {
      title: 'Check in with medical assistant/nurses',
      done: false,
      priority: 'Normal',
    },
    { title: 'Schedule surgery or procedures', done: false, priority: 'High' },
  ];

  onTodoToggled(todo: any) {
    console.log('Todo toggled:', todo);
  }

  onTodosUpdated(updatedTodos: any[]) {
    console.log('Todos updated:', updatedTodos);
  }
  // TODO end

  ngOnInit() {
    this.chart1();
    this.chart2();
    this.chart3();
  }
  private chart1() {
    this.areaChartOptions = {
      series: [
        {
          name: 'New Patients',
          data: [31, 40, 28, 51, 42, 85, 77],
        },
        {
          name: 'Old Patients',
          data: [11, 32, 45, 32, 34, 52, 41],
        },
      ],
      chart: {
        height: 330,
        type: 'area',
        toolbar: {
          show: false,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#7D4988', '#66BB6A'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      xaxis: {
        type: 'datetime',
        categories: [
          '2018-09-19T00:00:00.000Z',
          '2018-09-19T01:30:00.000Z',
          '2018-09-19T02:30:00.000Z',
          '2018-09-19T03:30:00.000Z',
          '2018-09-19T04:30:00.000Z',
          '2018-09-19T05:30:00.000Z',
          '2018-09-19T06:30:00.000Z',
        ],
      },
      legend: {
        show: true,
        position: 'top',
        horizontalAlign: 'center',
        offsetX: 0,
        offsetY: 0,
      },

      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          format: 'dd/MM/yy HH:mm',
        },
      },
    };
  }
  private chart2() {
    this.radialChartOptions = {
      series: [44, 55, 67],
      chart: {
        height: 265,
        type: 'radialBar',
      },
      plotOptions: {
        radialBar: {
          dataLabels: {
            name: {
              fontSize: '22px',
            },
            value: {
              fontSize: '16px',
            },
            total: {
              show: true,
              label: 'Total',
              formatter: function () {
                return '249';
              },
            },
          },
        },
      },
      colors: ['#ffc107', '#3f51b5', '#8bc34a'],

      labels: ['Face TO Face', 'E-Consult', 'Available'],
    };
  }
  private chart3() {
    this.linechartOptions = {
      series: [
        {
          name: 'Male',
          data: [44, 55, 57, 56, 61, 58],
        },
        {
          name: 'Female',
          data: [76, 85, 101, 98, 87, 105],
        },
      ],
      chart: {
        type: 'bar',
        height: 350,
        dropShadow: {
          enabled: true,
          color: '#000',
          top: 18,
          left: 7,
          blur: 10,
          opacity: 0.2,
        },
        toolbar: {
          show: false,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#786BED', '#AEAEAE'],
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '55%',
          borderRadius: 5,
        },
      },
      dataLabels: {
        enabled: false,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      stroke: {
        show: true,
        width: 2,
        colors: ['transparent'],
      },
      xaxis: {
        categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
      },
      yaxis: {},
      fill: {
        opacity: 1,
      },
      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }
}
