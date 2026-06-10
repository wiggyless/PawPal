import { DialogRef } from '@angular/cdk/dialog';
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { catchError, throwError } from 'rxjs';
import { RateLimitDialog } from '../../modules/shared/rate-limit-dialog/rate-limit-dialog/rate-limit-dialog';

export const rateLimitInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 429) {
        const serverMessage = error.error?.message || 'Too many requests. Please try again later.';

        const dialog = inject(MatDialog);
        dialog.open(RateLimitDialog);
        const retryAfter = error.headers.get('Retry-After');
        if (retryAfter) {
          console.warn(`Safe to retry after ${retryAfter} seconds.`);
        }
      }
      return throwError(() => error);
    }),
  );
};
