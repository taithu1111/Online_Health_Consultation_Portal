import { Component } from '@angular/core';
import { HealthRecordService } from './health-record.service'; // Đảm bảo đường dẫn đúng
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-health-record-list',
    standalone: true,  // Đánh dấu là Standalone Component
    imports: [CommonModule],  // Import module cần thiết
    templateUrl: './health-record-list.component.html',
    styleUrls: ['./health-record-list.component.css']
})
export class HealthRecordListComponent {
    healthRecords: any[] = [];
    patientId: number = 1;  // ID bệnh nhân

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
