import { CommonModule, DatePipe } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { Subject } from 'rxjs';
// import { rowsAnimation } from '@shared/animations';
import { formatDate } from '@angular/common';
// import { TableExportUtil } from '@shared/utils';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatDialogModule } from '@angular/material/dialog';
import { AppointmentService } from './appointments.service';
import { Appointments } from './appointments.model';
import { DoctorAppointmentFormComponent } from './dialogs/form/form.component';
import { AppointmentDelete } from './dialogs/delete/delete.component';
import { Action } from 'rxjs/internal/scheduler/Action';
import { AuthService } from '@core';
@Component({
  selector: 'app-appointments',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    // DatePipe,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatMenuModule,
    MatSelectModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    BreadcrumbComponent,
    DoctorAppointmentFormComponent,
    AppointmentDelete,
  ],
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.scss'],
  providers: [{ provide: MAT_DATE_LOCALE, useValue: 'en-GB' }],
  // animations: [rowsAnimation],
})
export class AppointmentsComponent implements OnInit, OnDestroy {
  columnDefinitions = [
    // { def: 'id', label: 'ID', type: 'string', visible: false },
    { def: 'patientName', label: 'Patient Name', type: 'text', visible: true },
    { def: 'gender', label: 'Gender', type: 'string', visible: true },
    { def: 'appointmentDate', label: 'Appointment Date', type: 'date', visible: true },
    { def: 'appointmentTime', label: 'Time', type: 'time', visible: true },
    { def: 'email', label: 'Email', type: 'string', visible: true },
    { def: 'phoneNumber', label: 'PhoneNumber', type: 'string', visible: true },
    { def: 'address', label: 'Address', type: 'string', visible: true },
    { def: 'status', label: 'Status', type: 'text', visible: true },
    { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true },
  ];

  dataSource = new MatTableDataSource<Appointments>([]);
  isLoading = true;
  private destroy$ = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('filter') filter!: ElementRef<HTMLInputElement>;

  constructor(
    private authService: AuthService,
    private appointmentService: AppointmentService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  getDisplayedColumns(): string[] {
    return this.columnDefinitions
      .filter(cd => cd.visible)
      .map(cd => cd.def);
  }

  loadData(): void {
    // const doctorId = 2;
    const doctor = this.authService.getCurrentUser();
    const doctorId = doctor?.userId ?? 0;
    this.isLoading = true;
    this.appointmentService.getAppointmentsByDoctorId(doctorId)
      .subscribe({
        next: (list) => {
          const mapped = list.map(a => ({
            ...a,
            phoneNumber: a.phone || '',
            appointmentDate: formatDate(a.appointmentDateTime, 'yyyy-MM-dd', 'en-GB'),
            appointmentTime: formatDate(a.appointmentDateTime, 'HH:mm', 'en-GB'),
          }));
          this.dataSource.data = mapped;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.isLoading = false;
        },
        error: err => {
          console.error(err);
          this.snackBar.open('Không tải được lịch hẹn', '', { duration: 2000 });
          this.isLoading = false;
        }
      });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.dataSource.filter = filterValue;
  }

  openEdit(appointment: Appointments): void {
    const ref = this.dialog.open(DoctorAppointmentFormComponent, {
      data: {
        appointments: appointment,
        action: 'edit'
      },
      width: '400px',
    });

    // ref.afterClosed().subscribe((result: Appointments | undefined) => {
    //   if (result) {
    //     this.appointmentService.updateAppointment(result.id, result)
    //       .subscribe({
    //         next: () => {
    //           this.snackBar.open('Cập nhật thành công', '', { duration: 2000 });
    //           this.loadData();
    //         },
    //         error: () => this.snackBar.open('Cập nhật thất bại', '', { duration: 2000 })
    //       });
    //   }
    // });
    ref.afterClosed().subscribe((result: Appointments | undefined) => {
      if (!result) return;

      const updateDto = {
        patientId: result.patientID,
        doctorId: result.doctorID,
        appointmentDateTime: result.appointmentDateTime,
        status: result.status
      };

      this.appointmentService
        .updateAppointment(result.id, updateDto)
        .subscribe({
          next: () => {
            this.snackBar.open('Cập nhật thành công', '', { duration: 2000 });
            this.loadData();
          },
          error: err => {
            console.error('Update failed', err);
            this.snackBar.open('Cập nhật thất bại', '', { duration: 2000 });
          }
        });
    });
  }
  detailsCall(appointment: Appointments): void {
    // ví dụ: điều hướng tới trang chi tiết
    this.snackBar.open(`Chi tiết cuộc hẹn ID: ${appointment.id}`, '', { duration: 2000 });
  }

  deleteAppointment(row: Appointments) {
    const dialogRef = this.dialog.open(AppointmentDelete, { data: row });
    dialogRef.afterClosed().subscribe(removed => {
      if (removed) {
        this.dataSource.data = this.dataSource.data.filter(r => r.id !== row.id);
        this.refreshTable();
      }
    });
  }
  private refreshTable() {
    this.paginator.pageIndex = 0;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  exportCsv(data: any[], filename = 'export.csv') {
    const csv = data.map(row =>
      Object.values(row).map(v => `"${v}"`).join(',')
    ).join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = filename;
    link.click();
  }
  exportExcel(): void {
    const exportData = this.dataSource.filteredData.map(x => ({
      ID: x.id,
      'Patient Name': x.patientName,
      Status: x.status,
      'Date & Time': `${x.appointmentDateTime}`,
    }));
    this.exportCsv(exportData, 'appointments.csv');
  }
}
