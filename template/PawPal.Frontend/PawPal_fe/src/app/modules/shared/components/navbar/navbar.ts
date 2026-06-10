import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PublicRoutingModule } from '../../../public/public-routing-module';
import { CommonModule } from '@angular/common';
import { ClientModule } from '../../../client/client-module';
import { SharedModule } from 'primeng/api';
import { DyanmicThemeService } from '../../../../core/services/dynamic-theme.service';
import { NotificationService, AppNotification } from '../../../../core/services/notifications/notification.service';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
  imports: [PublicRoutingModule, SharedModule, DatePipe],
})
export class NavbarComponent {
  currentUser = inject(CurrentUserService);
  notificationService = inject(NotificationService);

  @Input() roleId: number | null = null;
  dynamicThemeService = inject(DyanmicThemeService);
  menuOpened = false;
  tipsMenuOpen = false;
  aboutUsMenuOpen = false;
  newsMenuOpen = false;
  theme: string = 'light';
  cd = inject(ChangeDetectorRef);
    notificationsOpen = false;
bannerDismissed = false;

enableNotifications() {
  console.log('enableNotifications clicked');
  this.notificationService.requestPermissionAndRegister();
}

dismissBanner() {
  this.bannerDismissed = true;
}
  toggleMenu(): void {
    this.menuOpened = !this.menuOpened;
  }

  toggleTipsMenu(): void {
    this.tipsMenuOpen = !this.tipsMenuOpen;
    this.aboutUsMenuOpen = false;
    this.newsMenuOpen = false;
  }

  toggleAboutUsMenu(): void {
    this.aboutUsMenuOpen = !this.aboutUsMenuOpen;
    this.tipsMenuOpen = false;
    this.newsMenuOpen = false;
  }

  toggleNewsMenu(): void {
    this.newsMenuOpen = !this.newsMenuOpen;
    this.tipsMenuOpen = false;
    this.aboutUsMenuOpen = false;
  }
  toggleTheme(): void {
    this.dynamicThemeService.toggleTheme();
  }
}

    toggleNotifications(): void {
    this.notificationsOpen = !this.notificationsOpen;
    if (this.notificationsOpen) {
      this.notificationService.markAllAsRead();
    }
  }

    onNotificationClick(notification: AppNotification): void {
    this.notificationsOpen = false;
    this.notificationService.navigateTo(notification);
  }

}
