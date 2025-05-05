import { Component, Input } from '@angular/core';
import { HealthRecordService } from '../healthrecord/health-record.service';
import { CommonModule } from '@angular/common'; // Để sử dụng ngModel
import { FormsModule } from '@angular/forms'; // Import FormsModule

@Component({
    selector: 'app-health-record-form',
    standalone: true,
    imports: [CommonModule, FormsModule], // Đảm bảo FormsModule được thêm vào
    templateUrl: './health-record-form.component.html',
    styleUrls: ['./health-record-form.component.css']
})
export class HealthRecordFormComponent {
    @Input() healthRecordId: number | null = null;
    healthRecord: any = { recordType: '', fileUrl: '', createdAt: new Date() };

    constructor(private healthRecordService: HealthRecordService) { }

    ngOnInit(): void {
        if (this.healthRecordId) {
            this.getHealthRecord();
        }
    }

    getHealthRecord(): void {
        if (this.healthRecordId !== null) {  // Kiểm tra lại trước khi gọi API
            this.healthRecordService.getHealthRecords(this.healthRecordId)
                .subscribe(record => this.healthRecord = record);
        }
    }

    saveHealthRecord(): void {
        if (this.healthRecordId !== null) {
            this.healthRecordService.updateHealthRecord(this.healthRecordId, this.healthRecord)
                .subscribe(() => alert('Cập nhật hồ sơ thành công'));
        } else {
            this.healthRecordService.createHealthRecord(this.healthRecord)
                .subscribe(() => alert('Tạo mới hồ sơ thành công'));
        }
    }
}
