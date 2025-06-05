import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { fromEvent, Subject } from 'rxjs';
import {
  MAT_DATE_LOCALE,
  MatOptionModule,
  MatRippleModule,
} from '@angular/material/core';
import { UpcomingAppointment } from './upcoming-appointment.model';
import { formatDate, DatePipe, CommonModule, NgClass } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { rowsAnimation, TableExportUtil } from '@shared';
import { FeatherIconsComponent } from '@shared/components/feather-icons/feather-icons.component';
import { FormDialogComponent } from './dialogs/form-dialog/form-dialog.component';
import { UpcomingAppointmentDeleteComponent } from './dialogs/delete/delete.component';
import { Direction } from '@angular/cdk/bidi';
import { AppointmentService } from '../appointment-v1.service';
import { AuthService } from '../../../core/service/auth.service';

@Component({
  selector: 'app-upcoming-appointment',
  templateUrl: './upcoming-appointment.component.html',
  styleUrls: ['./upcoming-appointment.component.scss'],
  providers: [{ provide: MAT_DATE_LOCALE, useValue: 'en-GB' }, DatePipe],
  animations: [rowsAnimation],
  imports: [
    BreadcrumbComponent,
    FeatherIconsComponent,
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatSelectModule,
    ReactiveFormsModule,
    FormsModule,
    MatOptionModule,
    MatCheckboxModule,
    MatTableModule,
    MatSortModule,
    MatRippleModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatPaginatorModule,
    DatePipe,
  ]
})
export class UpcomingAppointmentComponent implements OnInit, OnDestroy {
  columnDefinitions = [
    { def: 'select', label: 'Checkbox', type: 'check', visible: true },
    { def: 'id', label: 'ID', type: 'number', visible: false },
    // { def: 'patientId', label: 'Patient ID', type: 'number', visible: true },
    // { def: 'doctorId', label: 'Doctor ID', type: 'number', visible: true },
    // { def: 'patientName', label: 'Patient', type: 'text', visible: true },
    { def: 'doctorName', label: 'Doctor', type: 'text', visible: true },
    { def: 'appointmentDate', label: 'Date', type: 'date', visible: true },
    { def: 'appointmentTime', label: 'Time', type: 'time', visible: true },
    { def: 'status', label: 'Status', type: 'text', visible: true },
    { def: 'type', label: 'Type', type: 'text', visible: true },
    { def: 'notes', label: 'Notes', type: 'text', visible: true },
    { def: 'diagnosis', label: 'Diagnosis', type: 'text', visible: true },
    { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true },
  ];

  dataSource = new MatTableDataSource<UpcomingAppointment>([]);
  selection = new SelectionModel<UpcomingAppointment>(true, []);
  isLoading = true;
  private destroy$ = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatMenuTrigger) menuTrigger!: MatMenuTrigger;
  contextMenuPosition = { x: '0px', y: '0px' };
  contextMenuItem!: UpcomingAppointment;

  constructor(
    private appointmentService: AppointmentService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private datePipe: DatePipe,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.loadData();
  }
  refresh() {
    this.loadData();
  }
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  exportExcel() {

  }
  getDisplayedColumns(): string[] {
    return this.columnDefinitions.filter(cd => cd.visible).map(cd => cd.def);
  }

  loadData() {
    const userId = Number(this.authService.getCurrentUser()?.userId);
    this.isLoading = true;

    this.authService.getPatientIdByUserId(userId).subscribe({
      next: (patientId: number) => {
        this.appointmentService.getAppointmentsByPatientId(patientId).subscribe({
          next: (apiData) => {
            this.dataSource.data = apiData.map((item: any) => {
              const dt = new Date(item.appointmentDateTime);
              return {
                id: item.id,
                doctorName: item.doctorName,
                appointmentDate: this.datePipe.transform(dt, 'yyyy-MM-dd')!,
                appointmentTime: this.datePipe.transform(dt, 'HH:mm:ss')!,
                status: item.status ?? 'Pending',
                type: item.type,
                notes: item.notes,
                diagnosis: item.diagnosis ?? 'No Diagnosis',
                doctor: item.doctor || null,
                date: this.datePipe.transform(dt, 'yyyy-MM-dd')!,
                time: this.datePipe.transform(dt, 'HH:mm:ss')!,
                injury: item.injury || null,
              } as unknown as UpcomingAppointment;
            });
            this.isLoading = false;
            this.refreshTable();
          },
          error: err => {
            console.error('Failed to load appointments', err);
            this.isLoading = false;
          }
        });
      },
      error: err => {
        console.error('Failed to get patientId from userId', err);
        this.isLoading = false;
      }
    });
  }


  private refreshTable() {
    this.paginator.pageIndex = 0;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    this.dataSource.filter = (event.target as HTMLInputElement).value.trim().toLowerCase();
  }

  addNew() { this.openDialog('add'); }
  editCall(row: UpcomingAppointment) { this.openDialog('edit', row); }

  openDialog(action: 'add' | 'edit', data?: UpcomingAppointment) {
    const dialogRef = this.dialog.open(FormDialogComponent, {
      width: '60vw',
      data: { upcomingAppointment: data, action },
    });
    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.loadData();
      }
    });
  }

  deleteItem(row: UpcomingAppointment) {
    const dialogRef = this.dialog.open(UpcomingAppointmentDeleteComponent, { data: row });
    dialogRef.afterClosed().subscribe(removed => {
      if (removed) {
        this.dataSource.data = this.dataSource.data.filter(r => r.id !== row.id);
        this.refreshTable();
      }
    });
  }

  onContextMenu(event: MouseEvent, item: UpcomingAppointment) {
    event.preventDefault();
    this.contextMenuItem = item;
    this.contextMenuPosition = { x: `${event.clientX}px`, y: `${event.clientY}px` };
    this.menuTrigger.openMenu();
  }

  showNotification(
    color: string,
    text: string,
    vertical: MatSnackBarVerticalPosition,
    horizontal: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', { duration: 2000, verticalPosition: vertical, horizontalPosition: horizontal, panelClass: color });
  }

  isAllSelected() {
    return this.selection.selected.length === this.dataSource.data.length;
  }
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach(row => this.selection.select(row));
  }
  removeSelectedRows() {
    const count = this.selection.selected.length;
    this.dataSource.data = this.dataSource.data.filter(item => !this.selection.selected.includes(item));
    this.selection.clear();
    this.showNotification('snackbar-danger', `${count} record(s) deleted`, 'bottom', 'center');
  }
}

