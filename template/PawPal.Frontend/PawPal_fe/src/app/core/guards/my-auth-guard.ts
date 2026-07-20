// src/app/core/guards/auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { CurrentUserService } from '../services/auth/current-user.service';

export const myAuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  const requireAuth = route.data['requireAuth'] === true;
  const isAuth = currentUser.isAuthenticated();
  // 1) If the route requires auth and the user isn't logged in -> login
  if (requireAuth && !isAuth) {
    router.navigate(['/auth/login']);
    return false;
  }

  // If it doesn't require auth -> let it through (public routes)
  if (!requireAuth) {
    return true;
  }

  // 2) Role check
  const user = currentUser.snapshot;
  if (!user) {
    router.navigate(['/auth/login']);
    return false;
  }
  const requireRoleId = route.data['requireRoleId'];
  if (requireRoleId !== undefined && user.roleid !== requireRoleId) {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }
  if (route.data['path'] == 'client' && user.roleid != 2) {
    router.navigate(['/']);
    return false;
  }
  return true;
};

export interface MyAuthRouteData {
  requireAuth?: boolean;
  requireRoleId?: number;
}
