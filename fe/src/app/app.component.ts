import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { HealthRecordService } from '../app/healthrecord/health-record.service';
import { CommonModule } from '@angular/common';
import { HealthRecordFormComponent } from "./healthrecord/health-record-form.component";
import { HealthRecordListComponent } from "./healthrecord/health-record-list.component";  // Để sử dụng ngIf, ngFor...

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, HttpClientModule, HealthRecordFormComponent, HealthRecordListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  healthRecords: any[] = [];
  patientId: number = 1; // ID bệnh nhân, bạn có thể lấy từ thông tin đăng nhập

  constructor(private healthRecordService: HealthRecordService) { }

  ngOnInit(): void {
    this.getHealthRecords();
  }

  getHealthRecords(): void {
    this.healthRecordService.getHealthRecords(this.patientId)
      .subscribe(records => this.healthRecords = records);
  }

  deleteHealthRecord(id: number): void {
    this.healthRecordService.deleteHealthRecord(id)
      .subscribe(() => this.getHealthRecords());
  }
}
