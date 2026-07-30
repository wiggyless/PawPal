// core/guards/my-auth-guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthTimeoutService } from '../services/auth/auth-timeout.service';

export const myExpireGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthTimeoutService);
  const router = inject(Router);

  if (authService.isTokenExpired()) {
    authService.handleLogout();
    return router.createUrlTree(['/auth']);
  }

  return true;
};
