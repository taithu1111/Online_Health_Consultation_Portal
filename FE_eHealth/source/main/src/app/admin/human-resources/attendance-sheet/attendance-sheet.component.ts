import { Component, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { MatSelectModule } from '@angular/material/select';
import { NgClass } from '@angular/common';

@Component({
    selector: 'app-attendance-sheet',
    templateUrl: './attendance-sheet.component.html',
    styleUrls: ['./attendance-sheet.component.scss'],
    imports: [
        BreadcrumbComponent,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        ReactiveFormsModule,
        MatDatepickerModule,
        MatIconModule,
        MatButtonModule,
        MatCardModule,
        NgClass,
    ]
})
export class AttendanceSheetComponent implements OnInit {
  attendanceForm: UntypedFormGroup;
  searchForm: UntypedFormGroup;
  employees: Array<{
    name: string;
    avatar: string;
    year: number;
    month: string;
    attendanceStatus: string[];
  }> = [];
  filteredEmployees: Array<any> = [];
  years: number[] = [2023, 2024, 2025]; // Available years
  months: string[] = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];
  selectedYear: number = 2024; // Default year
  selectedMonth: string = 'November'; // Default month

  constructor(private http: HttpClient) {
    this.attendanceForm = new UntypedFormGroup({
      fromDate: new UntypedFormControl(null, Validators.required),
      toDate: new UntypedFormControl(null, Validators.required),
    });

    // Search form for year and month
    this.searchForm = new UntypedFormGroup({
      year: new UntypedFormControl(this.selectedYear, Validators.required),
      month: new UntypedFormControl(this.selectedMonth, Validators.required),
    });
  }

  ngOnInit(): void {
    // Fetch employee data from JSON
    this.http
      .get<
        {
          name: string;
          avatar: string;
          year: number;
          month: string;
          attendanceStatus: string[];
        }[]
      >('assets/data/attendance-sheet.json')
      .subscribe((data) => {
        this.employees = data;
        this.filteredEmployees = [...this.employees]; // Initially display all employees
      });
  }

  onSearch(): void {
    // Get selected year and month from the form
    const { year, month } = this.searchForm.value;
    this.selectedYear = year;
    this.selectedMonth = month;

    // Filter employees based on the selected year and month
    this.filteredEmployees = this.employees.filter(
      (employee) => employee.year === year && employee.month === month
    );
  }
}
