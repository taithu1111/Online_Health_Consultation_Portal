import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { Subject, debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
import { Doctors } from './doctors.model';
import { CommonModule, formatDate } from '@angular/common';
import { TableExportUtil } from '@shared';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FeatherIconsComponent } from '@shared/components/feather-icons/feather-icons.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { DoctorsService } from './doctors.service';
import { PaginationFilter, PaginationParams } from '@core/models/pagination.model';
import { AllDoctorFormDialogComponent } from './dialogs/form-dialog/form-dialog.component';
import { AllDoctorsDeleteComponent } from './dialogs/delete/delete.component';
import { SpecializationService } from '@core/service/specialization.service';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-alldoctors',
  templateUrl: './alldoctors.component.html',
  styleUrls: ['./alldoctors.component.scss'],
  imports: [
    BreadcrumbComponent,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatTableModule,
    FeatherIconsComponent,
    CommonModule,
    MatIconModule,
    MatTooltipModule,
    MatCheckboxModule,
    FormsModule,
    MatOptionModule,
    NgxMatSelectSearchModule,
    MatSelectModule,
    MatFormFieldModule,
    ReactiveFormsModule,
  ]
})
export class AllDoctorsComponent implements OnInit, OnDestroy {
  columnDefinitions = [
    { def: 'select', label: 'Checkbox', type: 'check', visible: true },
    { def: 'name', label: 'Name', type: 'text', visible: true },
    { def: 'email', label: 'Email', type: 'email', visible: true },
    { def: 'phone', label: 'Phone', type: 'phone', visible: true },
    { def: 'specialization', label: 'Specialization', type: 'text', visible: true },
    { def: 'experienceYears', label: 'Experience (Years)', type: 'number', visible: true },
    { def: 'consultationFee', label: 'Fee', type: 'currency', visible: true },
    { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true },
  ];

  specializations: any[] = [];
  filteredSpecializations: any[] = [];
  specializationSearchControl = new FormControl();
  dataSource = new MatTableDataSource<Doctors>();
  selection = new SelectionModel<Doctors>(true, []);
  isLoading = true;
  private destroy$ = new Subject<void>();
  isLoadingSpecializations = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('filter') filter!: ElementRef;
  @ViewChild('columnSelect') columnSelect!: MatSelect;

  constructor(
    public dialog: MatDialog,
    private doctorService: DoctorsService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
    private specializationService: SpecializationService
  ) {
    console.log('SpecializationService:', this.specializationService); // Should log the service instance
  }

  filtersForm = this.fb.group({
    specializations: this.fb.control<number[]>([]),
    minExperienceYears: this.fb.control<number | null>(null),
    language: this.fb.control<string>(''),
    strictSpecializationFilter: this.fb.control<boolean>(false)
  });

  async ngOnInit() {
    try {
      await this.loadSpecializations();
      this.loadData();
    } catch (error) {
      console.error('Initialization failed:', error);
      // Data loading will still attempt but with empty specializations
      this.loadData();
    }
    this.setupSpecializationFilter();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setupSpecializationFilter() {
    this.specializationSearchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(value => {
          const isEmpty = !value || value.trim().length === 0;
          return isEmpty
            ? this.specializationService.getAllSpecializations()
            : this.specializationService.searchSpecializations(value);
        })
      )
      .subscribe(specializations => {
        this.filteredSpecializations = specializations;
      });
  }

  private loadSpecializations(): Promise<void> {
    return new Promise((resolve, reject) => {
      console.log('Attempting to load specializations...'); // Debug log

      const sub = this.specializationService.getAllSpecializations().subscribe({
        next: (res) => {
          console.log('Specializations loaded:', res); // Debug log
          this.specializations = res || [];
          this.filteredSpecializations = [...this.specializations];
          resolve();
        },
        error: (err) => {
          console.error('Error loading specializations:', err);
          reject(err);
        }
      });

      // Add to destroy$ to clean up subscription
      this.destroy$.subscribe(() => sub.unsubscribe());
    });
  }

  getFilters(): PaginationFilter {
    return {
      specializations: this.filtersForm.value.specializations || [],
      minExperienceYears: this.filtersForm.value.minExperienceYears || undefined,
      language: this.filtersForm.value.language || undefined,
      strictSpecializationFilter: this.filtersForm.value.strictSpecializationFilter || false
    };
  }

  applyFilter() {
    const pagination = new PaginationParams(this.paginator?.pageIndex ?? 0, this.paginator?.pageSize ?? 10);
    const filters = this.getFilters();
    this.isLoading = true;
    // console.log("applyfilter working uhh: " + JSON.stringify(filters));

    this.doctorService.getPaginatedDoctors(pagination, filters).subscribe({
      next: (res) => {
        this.dataSource.data = res.items;
        this.isLoading = false;
        this.refreshTable();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  loadData() {
    this.isLoading = true;
    const pagination = new PaginationParams(0, 10);
    const filters = this.getFilters();
    this.doctorService.getPaginatedDoctors(pagination, filters).subscribe({
      next: (res) => {
        this.dataSource.data = res.items;
        this.isLoading = false;
        this.refreshTable();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  openColumnSelect() {
    this.columnSelect.open();
  }

  private setupFilter() {
    this.dataSource.filterPredicate = (data: Doctors, filter: string) => {
      const dataStr = Object.keys(data)
        .reduce((currentTerm, key) => {
          return currentTerm + (data as any)[key] + '◬';
        }, '')
        .toLowerCase();
      return dataStr.indexOf(filter) !== -1;
    };
  }

  refresh() {
    this.loadData();
  }

  getDisplayedColumns(): string[] {
    return this.columnDefinitions
      .filter((cd) => cd.visible)
      .map((cd) => cd.def);
  }

  editCall(row: Doctors) {
    const dialogRef = this.dialog.open(AllDoctorFormDialogComponent, {
      data: {
        user: {
          id: row.userId,
          fullName: row.fullName,
          email: row.email,
          phoneNumber: row.phone,
          specializationId: row.specialization,
          experienceYears: row.experienceYears,
          consultationFee: row.consultationFee,
          rating: row.rating,
          languages: row.languages,
          bio: row.bio,
          role: 'Doctor'
        },
        action: 'edit'
      },
      width: '800px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  deleteItem(row: Doctors) {
    const dialogRef = this.dialog.open(AllDoctorsDeleteComponent, {
      data: row,
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dataSource.data = this.dataSource.data.filter(d => d.id !== row.id);
        this.showNotification('snackbar-success', 'Doctor deleted successfully', 'bottom', 'center');
      }
    });
  }

  private refreshTable() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  showNotification(
    colorName: string,
    text: string,
    placementFrom: 'top' | 'bottom',
    placementAlign: 'left' | 'center' | 'right'
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
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  removeSelectedRows() {
    const selectedIds = this.selection.selected.map(d => d.id);
    this.doctorService.deleteDoctors(selectedIds[0]).subscribe({
      next: () => {
        this.dataSource.data = this.dataSource.data.filter(d => !selectedIds.includes(d.id));
        this.selection.clear();
        this.showNotification('snackbar-success', `${selectedIds.length} doctor(s) deleted`, 'bottom', 'center');
      },
      error: (err: Error) => {
        console.log(err);
        this.showNotification('snackbar-error', 'Failed to delete doctors', 'bottom', 'center');
      }
    });
  }

  exportExcel() {
    if (!this.dataSource?.filteredData) return;

    const exportData = this.dataSource.filteredData.map(doctor => {
      const specializationNames = (doctor.specialization || [])
        .map(id => {
          const spec = this.specializations?.find(s => s.id === id);
          return spec ? spec.name : '';
        })
        .filter(name => name)
        .join(', ');

      return {
        'Name': doctor.fullName || 'N/A',
        'Email': doctor.email || 'N/A',
        'Mobile': doctor.phone || 'N/A',
        'Specialization': specializationNames || 'N/A',
        'Experience (Years)': doctor.experienceYears || 0,
        'Consultation Fee': doctor.consultationFee || 0
      };
    });

    TableExportUtil.exportToExcel(exportData, 'doctors');
  }

  contextMenuPosition = { x: '0px', y: '0px' };

  onContextMenu(event: MouseEvent, item: Doctors) {
    event.preventDefault();
    this.contextMenuPosition = {
      x: `${event.clientX}px`,
      y: `${event.clientY}px`
    };
  }
}