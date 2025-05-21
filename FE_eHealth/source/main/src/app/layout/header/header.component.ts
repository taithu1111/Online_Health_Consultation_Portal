import { Component, Inject, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { DOCUMENT, NgClass } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { ConfigService } from '@config';
import { AuthService, InConfiguration, LanguageService, RightSidebarService } from '@core';
import { UnsubscribeOnDestroyAdapter } from '@shared';
import { User, UserService } from '@core/service/user.service';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { NgScrollbar } from 'ngx-scrollbar';
import { FeatherIconsComponent } from '@shared/components/feather-icons/feather-icons.component';
import { MatIconModule } from '@angular/material/icon';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { MatToolbarModule } from '@angular/material/toolbar';

interface Notifications {
  message: string;
  time: string;
  icon: string;
  color: string;
  status: string;
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  imports: [
    RouterLink,
    NgClass,
    MatButtonModule,
    MatMenuModule,
    NgScrollbar,
    FeatherIconsComponent,
    MatIconModule,
    MatToolbarModule,
  ]
})
export class HeaderComponent extends UnsubscribeOnDestroyAdapter implements OnInit {
  public config!: InConfiguration;
  userRole?: string;
  userImg?: string;
  userEmail?: string;
  userName?: string;
  homePage?: string;
  isNavbarCollapsed = true;
  flagvalue: string | string[] | undefined;
  countryName: string | string[] = [];
  langStoreValue?: string;
  defaultFlag?: string;
  isOpenSidebar?: boolean;
  docElement?: HTMLElement;
  isFullScreen = false;
  public settingLink = '';

  user?: User; // thêm biến user để chứa dữ liệu user lấy về

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
    public elementRef: ElementRef,
    private rightSidebarService: RightSidebarService,
    private configService: ConfigService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,      // inject ActivatedRoute
    private userService: UserService,   // inject UserService
    public languageService: LanguageService
  ) {
    super();
  }

  notifications: Notifications[] = [];

  ngOnInit() {
    this.userRole = this.authService.getCurrentUserRole()?.toLowerCase() || '';
    this.config = this.configService.configData;
    console.log('User image:', this.userImg);
    this.userService.getProfile().subscribe({
      next: data => {
        this.user = data;
        this.userName = data.fullName;
        this.userEmail = data.email;
        this.userImg = (data as any).img;
        const userRole = data.role || '';
        this.setHomePageAndSettingLink(userRole);
      },
      error: err => console.error('Lỗi lấy user:', err)
    });

    this.docElement = this.document.documentElement;

    const token = this.authService.currentToken;
    if (token) {
      this.userName = this.authService.getFullNameFromToken(token);
    }
  }


  private setHomePageAndSettingLink(userRole: string) {
    if (userRole === 'Admin') {
      this.homePage = 'admin/dashboard/main';
    } else if (userRole === 'Patient') {
      this.homePage = 'patient/dashboard';
      this.settingLink = 'patient/settings';
    } else if (userRole === 'Doctor') {
      this.homePage = 'doctor/dashboard';
      this.settingLink = 'doctor/settings';
    } else {
      this.homePage = 'admin/dashboard/main';
    }
  }

  callFullscreen() {
    if (!this.isFullScreen) {
      if (this.docElement?.requestFullscreen != null) {
        this.docElement.requestFullscreen();
      }
    } else {
      this.document.exitFullscreen();
    }
    this.isFullScreen = !this.isFullScreen;
  }

  mobileMenuSidebarOpen(event: Event, className: string) {
    const hasClass = (event.target as HTMLElement).classList.contains(className);
    if (hasClass) {
      this.renderer.removeClass(this.document.body, className);
    } else {
      this.renderer.addClass(this.document.body, className);
    }
  }

  callSidemenuCollapse() {
    const hasClass = this.document.body.classList.contains('side-closed');
    if (hasClass) {
      this.renderer.removeClass(this.document.body, 'side-closed');
      this.renderer.removeClass(this.document.body, 'submenu-closed');
      localStorage.setItem('collapsed_menu', 'false');
    } else {
      this.renderer.addClass(this.document.body, 'side-closed');
      this.renderer.addClass(this.document.body, 'submenu-closed');
      localStorage.setItem('collapsed_menu', 'true');
    }
  }

  logout() {
    this.subs.sink = this.authService.logout().subscribe(res => {
      if (!res.success) {
        this.router.navigate(['/authentication/signin']);
      }
    });
  }
}
