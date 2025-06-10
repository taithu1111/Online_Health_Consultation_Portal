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
import { PaymentService } from '../payment.service';
import { BillList, PaymentDto } from '../payment.model';
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
import { FormDialogComponent } from './dialog/form-dialog/form-dialog.component';
import { BillListDeleteComponent } from './dialog/delete/delete.component';
import { Direction } from '@angular/cdk/bidi';
import { AppointmentService } from '../../appointment/bookappointment/appointment.service';
import { forkJoin } from 'rxjs';
import { AuthService } from '../../../core/service/auth.service';

export interface AppointmentDto {
  doctorName: string;
  patientName: string;
  appointmentDateTime: string;
}

@Component({
  selector: 'app-bill-list',
  templateUrl: './bill-list.component.html',
  styleUrls: ['./bill-list.component.scss'],
  providers: [{ provide: MAT_DATE_LOCALE, useValue: 'en-GB' }],
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
    NgClass,
    MatRippleModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatPaginatorModule,
    DatePipe,
  ]
})
export class BillListComponent implements OnInit, OnDestroy {
  columnDefinitions = [
    { def: 'select', label: 'Checkbox', type: 'check', visible: true },
    { def: 'patientName', label: 'Patient Name', type: 'text', visible: true },
    { def: 'doctorName', label: 'Doctor Name', type: 'text', visible: true },
    { def: 'status', label: 'Status', type: 'text', visible: true },
    { def: 'date', label: 'Admission Date', type: 'date', visible: true },
    { def: 'initialAmount', label: 'Initial Amount', type: 'text', visible: true },
    { def: 'tax', label: 'Tax', type: 'text', visible: true },
    { def: 'discount', label: 'Discount', type: 'text', visible: true },
    { def: 'total', label: 'Total Amount', type: 'text', visible: true },
    { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true },
  ];

  dataSource = new MatTableDataSource<BillList>([]);
  selection = new SelectionModel<BillList>(true, []);
  contextMenuPosition = { x: '0px', y: '0px' };
  isLoading = true;
  private destroy$ = new Subject<void>();
  readonly DISCOUNT_AMT = 5;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('filter') filter!: ElementRef;
  @ViewChild(MatMenuTrigger) contextMenu?: MatMenuTrigger;

  constructor(
    public httpClient: HttpClient,
    public dialog: MatDialog,
    public billListService: PaymentService,
    private snackBar: MatSnackBar,
    private appointmentService: AppointmentService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  refresh() {
    this.loadData();
  }

  getDisplayedColumns(): string[] {
    return this.columnDefinitions
      .filter((cd) => cd.visible)
      .map((cd) => cd.def);
  }

  loadData() {
    this.billListService.getAllBillLists().subscribe({
      next: (payments: PaymentDto[]) => {
        const appointmentCalls = payments.map(p =>
          this.appointmentService.getAppointmentById(p.appointmentId)
        );

        forkJoin(appointmentCalls).subscribe({
          next: appointments => {
            this.dataSource.data = payments.map((p, idx) => {
              const appt = appointments[idx];
              const taxRate = 0.1;
              const taxAmt = +(p.amount * taxRate);
              const total = +(p.amount + taxAmt - this.DISCOUNT_AMT);

              return {
                id: p.id.toString(),
                img: 'assets/images/user/user1.jpg',
                patientName: appt?.patientName ?? 'N/A',
                doctorName: appt?.doctorName ?? 'Unknown',
                status: p.status ?? 'N/A',
                initialAmount: p.amount.toFixed(2),
                tax: `${((p.amount * taxRate) / p.amount * 100).toFixed(0)}%`,
                date: new Date(appt?.appointmentDateTime ?? new Date()).toISOString(),
                discount: this.DISCOUNT_AMT.toFixed(2),
                total: total.toFixed(2),
              };
            }) as BillList[];

            this.isLoading = false; // Set loading to false after data is loaded
          },
          error: (err) => {
            console.error('Failed to load appointments', err);
          }
        });
      },
      error: (err) => {
        console.error('Failed to load payments', err);
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
    const filterValue = (event.target as HTMLInputElement).value
      .trim()
      .toLowerCase();
    this.dataSource.filter = filterValue;
  }

  editCall(row: BillList) {
    this.openDialog('edit', row);
  }

  openDialog(action: 'add' | 'edit', data?: BillList) {
    let varDirection: Direction = 'ltr';
    if (localStorage.getItem('isRtl') === 'true') {
      varDirection = 'rtl';
    }

    const dialogRef = this.dialog.open(FormDialogComponent, {
      width: '60vw',
      maxWidth: '100vw',
      data: { billList: data, action },
      direction: varDirection,
      autoFocus: false,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) {
        return; // Người dùng hủy dialog, không làm gì cả
      }
      this.updateRecordStatus(result);
      this.refreshTable();
      this.showNotification(
        'black',
        'Edit Record Successfully...!!!',
        'bottom',
        'center'
      );
    });
  }

  private updateRecordStatus(updatedRecord: { id: string; status: string }) {
    const index = this.dataSource.data.findIndex(
      (record) => record.id === updatedRecord.id
    );
    if (index === -1) return;

    // Lấy bản ghi cũ (instance của BillList)
    const existing = this.dataSource.data[index];

    // Tạo một object dạng Partial<BillList>, dùng constructor để khởi tạo đầy đủ
    const partial: Partial<BillList> = {
      id: existing.id,
      img: existing.img,
      patientName: existing.patientName,
      doctorName: existing.doctorName,
      // Trường status mới:
      status: updatedRecord.status,
      initialAmount: existing.initialAmount,
      tax: existing.tax,
      date: existing.date,
      discount: existing.discount,
      total: existing.total
    };

    // Tạo instance mới của BillList, đảm bảo có đủ getRandomID() và logic trong constructor
    const merged = new BillList(partial);

    // Gán lại vào dataSource
    this.dataSource.data[index] = merged;

    // Cho table refresh
    this.dataSource._updateChangeSubscription();
  }


  deleteItem(row: BillList) {
    // Mở dialog, truyền vào data cần hiển thị (id, patientName, doctorName, total)
    const dialogRef = this.dialog.open(BillListDeleteComponent, {
      data: {
        id: row.id,
        patientName: row.patientName,
        doctorName: row.doctorName,
        total: row.total
      }
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      // result === true nghĩa là xóa thành công
      if (result === true) {
        // Loại bỏ dòng đó khỏi dataSource
        this.dataSource.data = this.dataSource.data.filter(
          (record) => record.id !== row.id
        );
        this.refreshTable();
      }
      // Nếu result === false hoặc undefined, không làm gì (user hủy hoặc xóa thất bại)
    });
  }

  showNotification(
    colorName: string,
    text: string,
    placementFrom: MatSnackBarVerticalPosition,
    placementAlign: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }

  isAllSelected() {
    return this.selection.selected.length === this.dataSource.data.length;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach((row) => this.selection.select(row));
  }

  removeSelectedRows() {
    const totalSelect = this.selection.selected.length;
    this.dataSource.data = this.dataSource.data.filter(
      (item) => !this.selection.selected.includes(item)
    );
    this.selection.clear();
    this.showNotification(
      'snackbar-danger',
      `${totalSelect} Record(s) Deleted Successfully...!!!`,
      'bottom',
      'center'
    );
  }
  exportExcel() {
    const exportData = this.dataSource.filteredData.map((x) => ({
      'Patient Name': x.patientName,
      'Doctor Name': x.doctorName,
      Status: x.status,
      'Admission Date': formatDate(new Date(x.date), 'yyyy-MM-dd', 'en') || '',
      'Initial Amount': x.initialAmount,
      Tax: x.tax,
      Discount: x.discount,
      'Total Amount': x.total,
    }));

    TableExportUtil.exportToExcel(exportData, 'excel');
  }

  onContextMenu(event: MouseEvent, item: BillList) {
    event.preventDefault();
    this.contextMenuPosition = {
      x: `${event.clientX}px`,
      y: `${event.clientY}px`,
    };
    if (this.contextMenu) {
      this.contextMenu.menuData = { item };
      this.contextMenu.menu?.focusFirstItem('mouse');
      this.contextMenu.openMenu();
    }
  }
}
