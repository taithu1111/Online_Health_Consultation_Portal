import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { fromEvent, Subject } from 'rxjs';
import { AllDoctorsFormComponent } from './dialogs/form-dialog/form-dialog.component';
import { AllDoctorsDeleteComponent } from './dialogs/delete/delete.component';
import {
  MAT_DATE_LOCALE,
  MatOptionModule,
  MatRippleModule,
} from '@angular/material/core';
import { DoctorsService } from './doctors.service';
import { Doctors } from './doctors.model';
import { rowsAnimation, TableExportUtil } from '@shared';
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
import { FeatherIconsComponent } from '@shared/components/feather-icons/feather-icons.component';
import { Direction } from '@angular/cdk/bidi';
import { PaginationFilter, PaginationParams } from '@core/models/pagination.model';

@Component({
    selector: 'app-viewdoctors',
    templateUrl: './alldoctors.component.html',
    styleUrls: ['./alldoctors.component.scss'],
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
export class AlldoctorsComponent implements OnInit, OnDestroy {
   columnDefinitions = [
    { def: 'select', label: 'Checkbox', type: 'check', visible: true },
    { def: 'fullName', label: 'Name', type: 'text', visible: true },
    { def: 'email', label: 'Email', type: 'email', visible: true },
    { def: 'specialization', label: 'Specialization', type: 'text', visible: true },
    { def: 'experienceYears', label: 'Experience (Years)', type: 'number', visible: true },
    { def: 'languages', label: 'Languages', type: 'text', visible: true },
    { def: 'consultationFee', label: 'Consultation Fee', type: 'currency', visible: true },
    { def: 'averageRating', label: 'Rating', type: 'number', visible: true },
    { def: 'bio', label: 'Bio', type: 'text', visible: false },
    { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true }
  ];

  dataSource = new MatTableDataSource<Doctors>([]);
  selection = new SelectionModel<Doctors>(true, []);
  isLoading = true;
  private destroy$ = new Subject<void>();

  // Pagination properties
  pagination = new PaginationParams();
  filters: PaginationFilter = {};
  totalItems = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private doctorsService: DoctorsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadData() {
    this.isLoading = true;
    this.doctorsService.getPaginatedDoctors(this.pagination, this.filters)
      .subscribe({
        next: (response) => {
          this.dataSource.data = response.items.map(d => new Doctors({
            ...d,
            img: 'assets/images/user/user1.jpg' // Adding default image for UI
          }));
          this.totalItems = response.totalCount;
          this.isLoading = false;
          this.setupTableFeatures();
        },
        error: (error) => {
          console.error('Error loading doctors:', error);
          this.isLoading = false;
        }
      });
  }

  private setupTableFeatures() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.filterPredicate = (data: Doctors, filter: string) => {
      return Object.values(data).some(
        (value) => value?.toString().toLowerCase().includes(filter.toLowerCase())
      );
    };
  }

  getDisplayedColumns(): string[] {
    return this.columnDefinitions
      .filter((cd) => cd.visible)
      .map((cd) => cd.def);
  }

  onPageChange(event: PageEvent) {
    this.pagination.page = event.pageIndex + 1;
    this.pagination.pageSize = event.pageSize;
    this.loadData();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  applyFilters() {
    this.pagination.page = 1; // Reset to first page when filters change
    this.loadData();
  }

  addNew() {
    const dialogRef = this.dialog.open(AllDoctorsFormComponent, {
      width: '60vw',
      data: { action: 'add' }
    });

    dialogRef.afterClosed().subscribe((result: Doctors) => {
      if (result) {
        this.doctorsService.addDoctors(result).subscribe({
          next: () => {
            this.loadData();
            this.showNotification('Doctor added successfully', 'success');
          },
          error: (err) => this.showNotification('Error adding doctor', 'error')
        });
      }
    });
  }

  editCall(row: Doctors) {
    const dialogRef = this.dialog.open(AllDoctorsFormComponent, {
      width: '60vw',
      data: { doctors: row, action: 'edit' }
    });

    dialogRef.afterClosed().subscribe((result: Doctors) => {
      if (result) {
        this.doctorsService.updateDoctors(result.id, result).subscribe({
          next: () => {
            this.loadData();
            this.showNotification('Doctor updated successfully', 'success');
          },
          error: (err) => this.showNotification('Error updating doctor', 'error')
        });
      }
    });
  }

  deleteItem(row: Doctors) {
    const dialogRef = this.dialog.open(AllDoctorsDeleteComponent, {
      data: row
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.doctorsService.deleteDoctors(row.id).subscribe({
          next: () => {
            this.loadData();
            this.showNotification('Doctor deleted successfully', 'success');
          },
          error: (err) => this.showNotification('Error deleting doctor', 'error')
        });
      }
    });
  }

  exportExcel() {
    const exportData = this.dataSource.filteredData.map((x) => ({
      'Name': x.name,
      'Email': x.email,
      'Specialization': x.specialization,
      'Experience (Years)': x.experienceYears,
      'Languages': x.languages,
      'Consultation Fee': x.consultationFee,
      'Rating': x.rating
    }));
  }

  showNotification(message: string, type: 'success' | 'error') {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: type === 'success' ? 'snackbar-success' : 'snackbar-error'
    });
  }

  isAllSelected() {
    return this.selection.selected.length === this.dataSource.data.length;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }
}
