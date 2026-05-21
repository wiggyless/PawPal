import { inject, Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Subscription, timer } from 'rxjs';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AuthFacadeService } from './auth-facade.service';
import { RefreshTokenCommand } from '../../../api-services/auth/auth-api.model';
import { SessionWarningDialog } from '../../../modules/shared/session/session-warning-dialog/session-warning-dialog';

@Injectable({
  providedIn: 'root', // Marks this service as a singleton available globally
})
export class AuthTimeoutService implements OnDestroy {
  private currentUser = inject(CurrentUserService);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private auth = inject(AuthFacadeService);
  private timeoutSubscription?: Subscription;

  // Change these values based on your needs
  private readonly WARNING_BUFFER_MS = 60000; // Warn user 5 minute before expiration

  constructor() {}

  /**
   * Starts tracking the active token expiration window.
   * Call this whenever a user logs in or refreshes the page.
   */
  startExpirationTracker(): void {
    this.stopExpirationTracker(); // Clear any existing timers first

    const token = localStorage.getItem('accessToken'); // Access your raw JWT string
    if (!token) return;

    const expirationTimeMs = this.getTokenExpirationDate(token);
    const currentTimeMs = Date.now();
    const timeRemainingMs = expirationTimeMs - currentTimeMs;

    // Determine when to display the warning dialog
    const delayUntilWarning = timeRemainingMs - this.WARNING_BUFFER_MS;

    if (delayUntilWarning > 0) {
      // Schedule the warning timer
      this.timeoutSubscription = timer(delayUntilWarning).subscribe(() => {
        this.showSessionWarning();
      });
    } else if (timeRemainingMs > 0) {
      // The session has less than 5 left right now
      this.showSessionWarning();
    } else {
      // Token is already expired, route straight out
      this.handleLogout();
    }
  }

  /**
   * Stops the active background tracking stream completely.
   */
  stopExpirationTracker(): void {
    if (this.timeoutSubscription) {
      this.timeoutSubscription.unsubscribe();
    }
  }

  /**
   * Decodes JWT token payload metadata to grab the accurate expiration timestamp
   */
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
    const dialogRef = this.dialog.open(SessionWarningDialog);
    const refresh: RefreshTokenCommand = {
      refreshToken: localStorage.getItem('refreshToken')!,
    };
    dialogRef.afterClosed().subscribe((extend) => {
      if (extend) {
        this.auth.timeoutRefresh(refresh);
        console.log('Extended session');
      }
    });
  }

  private handleLogout(): void {
    this.stopExpirationTracker();
    this.auth.logout();
    this.currentUser.getDefaultRoute();
    this.dialog.closeAll();
    this.router.navigate(['/auth/login']);
  }

  ngOnDestroy(): void {
    this.stopExpirationTracker();
  }
}
