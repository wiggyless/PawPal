import { inject, Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription, timer } from 'rxjs';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AuthFacadeService } from './auth-facade.service';
import { RefreshTokenCommand } from '../../../api-services/auth/auth-api.model';
import { DialoguePopupService } from '../../../api-services/dialogue-popup/dialogue-popup.service';

@Injectable({
  providedIn: 'root',
})
export class AuthTimeoutService implements OnDestroy {
  private currentUser = inject(CurrentUserService);
  private router = inject(Router);
  private dialoguePopup = inject(DialoguePopupService);
  private auth = inject(AuthFacadeService);
  private timeoutSubscription?: Subscription;
  private readonly WARNING_BUFFER_MS = 60000;

  constructor() {}

  startExpirationTracker(): void {
    this.stopExpirationTracker();
    const token = localStorage.getItem('accessToken');
    if (!token) return;
    const expirationTimeMs = this.getTokenExpirationDate(token);
    const currentTimeMs = Date.now();
    const timeRemainingMs = expirationTimeMs - currentTimeMs;
    const delayUntilWarning = timeRemainingMs - this.WARNING_BUFFER_MS;

    if (delayUntilWarning > 0) {
      this.timeoutSubscription = timer(delayUntilWarning).subscribe(() => {
        this.showSessionWarning();
      });
    } else if (timeRemainingMs > 0) {
      this.showSessionWarning();
    } else {
      this.handleLogout();
    }
  }

  stopExpirationTracker(): void {
    if (this.timeoutSubscription) {
      this.timeoutSubscription.unsubscribe();
    }
  }

  private getTokenExpirationDate(token: string): number {
    try {
      const payloadBase64 = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payloadBase64));
      if (!decodedPayload.exp) return 0;
      return decodedPayload.exp * 1000;
    } catch (error) {
      console.error('Failed to parse JWT token expiration metadata:', error);
      return 0;
    }
  }

  private showSessionWarning(): void {
    const refresh: RefreshTokenCommand = {
      refreshToken: localStorage.getItem('refreshToken')!,
    };

    this.dialoguePopup.warning(
      'Session Expiring',
      'Your session will soon come to an end! Do you want to extend your stay?',
      'Yes',
      'No',
      () => {
        this.auth.timeoutRefresh(refresh);
        console.log('Extended session');
      },
      () => {
      this.handleLogout();
    }
    );
  }

  private handleLogout(): void {
    this.stopExpirationTracker();
    this.auth.logout();
    this.currentUser.getDefaultRoute();
    this.router.navigate(['/auth/login']);
  }

  ngOnDestroy(): void {
    this.stopExpirationTracker();
  }
}