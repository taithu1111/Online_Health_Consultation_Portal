import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl } from '@angular/forms';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  UntypedFormControl,
  Validators
} from '@angular/forms';
import {
  MatDialogModule,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { forkJoin } from 'rxjs';
import {
  OwlDateTimeModule,
  OwlNativeDateTimeModule
} from '@danielmoncada/angular-datetime-picker';

import { CalendarService } from '../../calendar.service';
import {
  ScheduleDto,
  CreateScheduleCommand,
  UpdateScheduleCommand
} from '../../calendar.model';

export interface DialogData {
  action: 'create' | 'edit';
  calendar?: ScheduleDto;
  doctorId: number;
}

@Component({
  selector: 'app-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatIconModule,
    MatButtonModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule
  ],
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss']
})
export class FormDialogComponent {
  action: 'create' | 'edit';
  dialogTitle: string;
  calendarForm: UntypedFormGroup;
  showDeleteBtn: boolean;
  private calendar: ScheduleDto;

  constructor(
    private fb: UntypedFormBuilder,
    public dialogRef: MatDialogRef<FormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private calendarService: CalendarService
  ) {
    this.action = data.action;
    this.calendar = data.calendar ?? ({} as ScheduleDto);
    this.dialogTitle = this.action === 'edit' ? 'Edit Event' : 'New Event';
    this.showDeleteBtn = this.action === 'edit';
    console.log('FormDialogComponent received data:', data);

    // Khởi tạo form, map các field theo template
    this.calendarForm = this.fb.group({
      id: [this.calendar.id],
      title: [this.calendar.location ?? '', [Validators.required]],
      category: [this.calendar.description ?? ''],
      startDate: [
        this.calendar.startTime
          ? new Date(`1970-01-01T${this.calendar.startTime}`)
          : null,
        [Validators.required]
      ],
      endDate: [
        this.calendar.endTime
          ? new Date(`1970-01-01T${this.calendar.endTime}`)
          : null,
        [Validators.required]
      ],
      details: ['']
    });
  }

  getErrorMessage(ctrl: AbstractControl | null): string {
    if (!ctrl) return '';
    if (ctrl.hasError('required')) {
      return 'This field is required';
    }
    return '';
  }

  submit() {
    if (this.calendarForm.invalid) {
      return;
    }

    if (!this.data.doctorId) {
      console.error('Missing doctorId in dialog data');
      alert('Không có doctorId! Vui lòng kiểm tra lại.');
      return;
    }

    const f = this.calendarForm.value;
    const start = new Date(f.startDate);
    const end = new Date(f.endDate);

    if (this.action === 'edit') {
      const cmd: UpdateScheduleCommand = {
        id: f.id,
        date: start.toISOString().split('T')[0],
        startTime: start.toTimeString().slice(0, 8),
        endTime: end.toTimeString().slice(0, 8),
        location: f.title,
        description: f.details
      };
      this.calendarService
        .updateSchedule(cmd.id, cmd)
        .subscribe(() => this.dialogRef.close({ action: 'edit', data: cmd }));
    } else {
      // Nếu cùng ngày thì tạo 1 lịch như cũ
      if (start.toDateString() === end.toDateString()) {
        const cmd: CreateScheduleCommand = {
          doctorId: this.data.doctorId,
          date: start.toISOString().split('T')[0],
          startTime: start.toTimeString().slice(0, 8),
          endTime: end.toTimeString().slice(0, 8),
          location: f.title,
          description: f.details
        };
        this.calendarService
          .createSchedule(cmd)
          .subscribe((newId) => this.dialogRef.close({ action: 'create', data: { ...cmd, id: newId } }));
      } else {
        // Nếu khác ngày => tạo nhiều lịch
        const requests = [];
        const loopDate = new Date(start);
        while (loopDate <= end) {
          const cmd: CreateScheduleCommand = {
            doctorId: this.data.doctorId,
            date: loopDate.toISOString().split('T')[0],
            startTime: start.toTimeString().slice(0, 8),
            endTime: end.toTimeString().slice(0, 8),
            location: f.title,
            description: f.details
          };
          requests.push(this.calendarService.createSchedule(cmd));
          loopDate.setDate(loopDate.getDate() + 1);
        }

        forkJoin(requests).subscribe(() => {
          this.dialogRef.close({ action: 'create-multiple' });
        });
      }

      console.log('FormDialog received data:', this.data);
      console.log('DoctorId passed to dialog:', this.data.doctorId);
    }
  }

  deleteEvent() {
    const id = this.calendarForm.get('id')!.value;
    this.calendarService
      .deleteSchedule(id)
      .subscribe(() => this.dialogRef.close({ action: 'delete', id }));
  }

  cancel() {
    this.dialogRef.close();
  }

  private formatTime(d: Date): string {
    // trả về "HH:mm:ss"
    return d.toTimeString().slice(0, 8);
  }
}
