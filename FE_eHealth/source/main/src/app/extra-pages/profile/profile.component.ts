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
import { BackendEnvironment, environment } from 'environments/environment';

@Component({
  selector: 'app-profile',
  standalone: true,
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
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
export class ProfileComponent implements OnInit {
  profile: any;
  loading = true;
  error: string | null = null;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getProfile().subscribe({
      next: (data) => {
        // console.log("data: " + data);
        this.profile = data;
        // console.log("Profile:" + this.profile);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading profile:', err);
        this.error = 'Failed to load profile data.';
        this.loading = false;
      }
    });

    console.log(this.profile.ImageUrl)
  }
  
  get profileImageUrl(): string {
    if (this.profile?.imageUrl) {
      return `${BackendEnvironment.apiUrl}${this.profile.imageUrl}`;
    }
    return 'assets/images/user/default-user.jpg'; // Replace with your default image path
  }
}