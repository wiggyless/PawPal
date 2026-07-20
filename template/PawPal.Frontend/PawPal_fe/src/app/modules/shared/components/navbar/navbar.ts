import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PublicRoutingModule } from '../../../public/public-routing-module';
import { SharedModule } from 'primeng/api';
import {
  NotificationService,
  AppNotification,
} from '../../../../core/services/notifications/notification.service';
import { DatePipe } from '@angular/common';
import { Subject, Subscription, debounceTime } from 'rxjs';

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
  menuOpened = false;
  cd = inject(ChangeDetectorRef);
  notificationsOpen = false;
  bannerDismissed = false;
  private mouseLeave$ = new Subject<void>();
  private subscription: Subscription;
  enableNotifications() {
    this.notificationService.requestPermissionAndRegister();
  }

  dismissBanner() {
    this.bannerDismissed = true;
  }
  toggleMenu(): void {
    this.menuOpened = !this.menuOpened;
  }
  constructor() {
    this.subscription = this.mouseLeave$.pipe(debounceTime(100)).subscribe(() => {
      this.menuOpened = false;
    });
  }

  onMouseEnter(): void {
    this.subscription.unsubscribe();
    this.menuOpened = true;
    this.subscription = this.mouseLeave$.pipe(debounceTime(100)).subscribe(() => {
      this.menuOpened = false;
    });
  }

  onMouseLeave(): void {
    this.mouseLeave$.next();
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
