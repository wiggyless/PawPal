import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { DialoguePopupService } from '../../api-services/dialogue-popup/dialogue-popup.service';

export const rateLimitInterceptor: HttpInterceptorFn = (req, next) => {
  const dialoguePopup = inject(DialoguePopupService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 429) {
        const retryAfter = error.headers.get('Retry-After');

        dialoguePopup.warning(
          'Too Many Requests',
          retryAfter
            ? `Too many requests have been sent. Please wait ${retryAfter} seconds before trying again.`
            : 'Too many requests have been sent, please slow down.'
        );

        if (retryAfter) {
          console.warn(`Safe to retry after ${retryAfter} seconds.`);
        }
      }
      return throwError(() => error);
    }),
  );
};