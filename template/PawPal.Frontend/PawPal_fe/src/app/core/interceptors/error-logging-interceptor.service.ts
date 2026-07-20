import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToasterService } from '../services/toaster.service';

/**
 * HTTP interceptor that logs errors.
 *
 * IMPORTANT: This interceptor only LOGS errors, it does NOT show toasters.
 * Toaster notifications should be handled in individual components where you
 * can provide context-specific error messages.
 *
 * Features:
 * - Logs all HTTP errors to console
 * - Can be extended to send errors to logging service (Sentry, etc.)
 * - Re-throws errors so components can handle them
 */
export const errorLoggingInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Log error to console
      // In production, send to logging service like Sentry
      console.error('HTTP Error:', {
        url: req.url,
        method: req.method,
        status: error.status,
        statusText: error.statusText,
        message: error.message,
        error: error.error,
        timestamp: new Date().toISOString()
      });

      // Re-throw error so components can handle it
      // Components should show appropriate toaster messages with context
      return throwError(() => error);
    })
  );
};
