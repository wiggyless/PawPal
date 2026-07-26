import {
  HttpInterceptorFn,
  HttpErrorResponse,
  HttpRequest,
  HttpHandlerFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { AuthFacadeService } from '../services/auth/auth-facade.service';

let refreshInProgress = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthFacadeService);

  if (isAuthEndpoint(req.url) || isStaticFile(req.url)) {
    return next(req);
  }

  const accessToken = auth.getAccessToken();
  let authReq = req;

  if (accessToken) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  return next(authReq).pipe(
    catchError((err) => {
      if (err instanceof HttpErrorResponse && err.status === 401) {
        return handle401Error(authReq, next, auth);
      }
      return throwError(() => err);
    }),
  );
};

function isAuthEndpoint(url: string): boolean {
  return url.includes('/Auth/');
}

function isStaticFile(url: string): boolean {
  return /\.(jpg|jpeg|png|webp|gif|svg|png)$/i.test(url);
}

function handle401Error(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  auth: AuthFacadeService,
): Observable<any> {
  const refreshToken = auth.getRefreshToken();

  if (!refreshToken) {
    auth.redirectToLogin();
    return throwError(() => new Error('No refresh token'));
  }

  if (refreshInProgress) {
    // Only the final emission of a refresh cycle matters here: a non-null token (success),
    // or null once refreshInProgress has already flipped back to false (failure). The
    // in-progress null reset below is filtered out so it doesn't get mistaken for a failure.
    return refreshTokenSubject.pipe(
      filter((token) => token !== null || !refreshInProgress),
      take(1),
      switchMap((token) => {
        if (!token) {
          return throwError(() => new Error('Session refresh failed'));
        }
        return next(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }));
      }),
    );
  }

  refreshInProgress = true;
  refreshTokenSubject.next(null);

  return auth.refresh({ refreshToken, fingerprint: null }).pipe(
    switchMap((res) => {
      refreshInProgress = false;
      const newAccessToken = res.accessToken;
      refreshTokenSubject.next(newAccessToken);

      const clonedReq = req.clone({
        setHeaders: { Authorization: `Bearer ${newAccessToken}` },
      });

      return next(clonedReq);
    }),
    catchError((error) => {
      refreshInProgress = false;
      refreshTokenSubject.next(null);
      auth.redirectToLogin();
      return throwError(() => error);
    }),
  );
}
