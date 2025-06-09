import { Component } from '@angular/core';
import { Location } from '@angular/common'; // 👈 import Location

@Component({
  selector: 'app-page404',
  templateUrl: './page404.component.html',
  styleUrls: ['./page404.component.scss'],
  standalone: true,
  imports: []
})
export class Page404Component {
  constructor(private location: Location) {}

  goBack(): void {
    window.history.length > 1 ? this.location.back() : window.location.href = '/authentication/signin'; // 👈 go back to previous page
  }
}
