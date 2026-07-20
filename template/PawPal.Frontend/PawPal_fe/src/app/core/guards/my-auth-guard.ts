import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { CurrentUserService } from '../services/auth/current-user.service';

export const myAuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  const requireAuth = route.data['requireAuth'] === true;
  const isAuth = currentUser.isAuthenticated();
  if (requireAuth && !isAuth) {
    router.navigate(['/auth/login']);
    return false;
  }

  if (!requireAuth) {
    return true;
  }

  const user = currentUser.snapshot;
  if (!user) {
    router.navigate(['/auth/login']);
    return false;
  }

  if (route.data['paht'] == 'client' && user.roleid != 2) {
    router.navigate(['/']);
    return false;
  }
  return true;
};

export interface MyAuthRouteData {
  requireAuth?: boolean;
  requireRoleId?: number;
}

export function myAuthData(data: MyAuthRouteData): { auth: MyAuthRouteData } {
  return { auth: data };
}
