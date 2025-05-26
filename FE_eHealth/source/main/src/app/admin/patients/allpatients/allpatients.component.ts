import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { Subject, of } from 'rxjs';
import { Patient } from './patient.model';
import { PatientService } from './patient.service';
import { CommonModule, formatDate } from '@angular/common';
import { AllPatientFormDialogComponent } from './dialog/form-dialog/form-dialog.component';
import { AllPatientDeleteComponent } from './dialog/delete/delete.component';
import { TableExportUtil } from '@shared';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FeatherIconsComponent } from '@shared/components/feather-icons/feather-icons.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-allpatients',
  templateUrl: './allpatients.component.html',
  styleUrls: ['./allpatients.component.scss'],
  imports:[
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
    MatOptionModule
  ]
})
export class AllpatientsComponent implements OnInit, OnDestroy {
  columnDefinitions = [
    { def: 'select', label: 'Checkbox', type: 'check', visible: true },
    { def: 'fullName', label: 'Name', type: 'text', visible: true },
    { def: 'email', label: 'Email', type: 'email', visible: true },
    { def: 'gender', label: 'Gender', type: 'text', visible: true },
    { def: 'phone', label: 'Phone', type: 'phone', visible: true },
    { def: 'bloodType', label: 'Blood Type', type: 'text', visible: true },
    { def: 'address', label: 'Address', type: 'address', visible: true },
    { def: 'dateOfBirth', label: 'Date of Birth', type: 'date', visible: true },
    { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true },
  ];

  dataSource = new MatTableDataSource<Patient>();
  selection = new SelectionModel<Patient>(true, []);
  isLoading = true;
  private destroy$ = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('filter') filter!: ElementRef;
  @ViewChild('columnSelect') columnSelect!: MatSelect;

  constructor(
    public dialog: MatDialog,
    private patientService: PatientService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadData() {
    this.isLoading = true;
    this.patientService.getAllPatients(1, 100, '').subscribe({
      next: (patients) => {
        patients.forEach(p => {
          if (!p.dateOfBirth || isNaN(Date.parse(p.dateOfBirth))) {
            p.dateOfBirth = '';
          }
        });
        this.dataSource.data = patients;
        this.isLoading = false;
        this.refreshTable();
        this.setupFilter();
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
    this.dataSource.filterPredicate = (data: Patient, filter: string) => {
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

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.dataSource.filter = filterValue;
  }

  editCall(row: Patient) {
    const dialogRef = this.dialog.open(AllPatientFormDialogComponent, {
      data: { 
        user: {
          id: row.id,
          fullName: row.fullName,
          email: row.email,
          phoneNumber: row.phone,
          gender: row.gender,
          dateOfBirth: row.dateOfBirth,
          bloodType: row.bloodType,
          address: row.address,
          role: 'Patient'
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

  deleteItem(row: Patient) {
    const dialogRef = this.dialog.open(AllPatientDeleteComponent, {
      data: row,
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.patientService.deletePatient(row.id).subscribe({
          next: () => {
            this.dataSource.data = this.dataSource.data.filter(p => p.id !== row.id);
            this.showNotification('snackbar-success', 'Patient deleted successfully', 'bottom', 'center');
          },
          error: (err) => {
            this.showNotification('snackbar-error', 'Failed to delete patient', 'bottom', 'center');
          }
        });
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
    const selectedIds = this.selection.selected.map(p => p.id);
    this.patientService.deletePatient(selectedIds[0]).subscribe({
      next: () => {
        this.dataSource.data = this.dataSource.data.filter(p => !selectedIds.includes(p.id));
        this.selection.clear();
        this.showNotification('snackbar-success', `${selectedIds.length} patient(s) deleted`, 'bottom', 'center');
      },
      error: (err) => {
        this.showNotification('snackbar-error', 'Failed to delete patients', 'bottom', 'center');
      }
    });
  }

  exportExcel() {
    const exportData = this.dataSource.filteredData.map(patient => ({
      'Name': patient.fullName,
      'Email': patient.email,
      'Phone': patient.phone,
      'Gender': patient.gender,
      'Blood Type': patient.bloodType,
      'Address': patient.address,
      'Date of Birth': patient.dateOfBirth ? formatDate(patient.dateOfBirth, 'yyyy-MM-dd', 'en') : ''
    }));
    TableExportUtil.exportToExcel(exportData, 'patients');
  }

  contextMenuPosition = { x: '0px', y: '0px' };

  // Add this method to handle context menu events
  onContextMenu(event: MouseEvent, item: Patient) {
    event.preventDefault();
    this.contextMenuPosition = {
      x: `${event.clientX}px`,
      y: `${event.clientY}px`
    };
    
    // If you want to actually show a context menu, you would need to:
    // 1. Add @ViewChild(MatMenuTrigger) contextMenu!: MatMenuTrigger;
    // 2. Implement the menu trigger logic here
    // this.contextMenu.menuData = { item };
    // this.contextMenu.openMenu();
  }
}