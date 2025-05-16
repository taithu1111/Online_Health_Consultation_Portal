import { Component } from '@angular/core';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-medical-records',
  templateUrl: './health-records.component.html',
  styleUrls: ['./health-records.component.scss'],
  imports: [BreadcrumbComponent]
})
export class MedicalRecordsComponent {
  constructor() {
    // constructor code
  }
}
