import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '@core/service/user.service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.scss'],
  imports: [
    CommonModule,
    BreadcrumbComponent,
    MatTabsModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
  ]
})
export class DoctorsComponent implements OnInit {
  profile: any;
  loading = true;
  error: string | null = null;

  constructor(
    private userService: UserService,
    private router: Router) { }

  goToSettings() {
    this.router.navigate(['doctor/settings']);
  }
  ngOnInit(): void {
    this.userService.getProfile().subscribe({
      next: (data) => {
        this.profile = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading profile:', err);
        this.error = 'Failed to load profile data.';
        this.loading = false;
      }
    });
  }
}