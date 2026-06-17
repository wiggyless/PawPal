import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { LoadingBarService } from '../services/loading-bar.service';

export const loadingBarInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingBar = inject(LoadingBarService);

  // Skip loading bar for certain endpoints
  if (shouldSkipLoadingBar(req.url)) {
    return next(req);
  }

  // Show loading bar (increments counter)
  loadingBar.show();

  // Process request and hide loading bar when done
  return next(req).pipe(
    finalize(() => {
      // Hide loading bar (decrements counter)
      // Will only hide when all requests are complete
      loadingBar.hide();
    })
  );
};

/**
 * Determine if loading bar should be skipped for this URL
 *
 * Skip loading bar for:
 * - Polling endpoints (frequent background requests)
 * - Health check endpoints
 * - Analytics/tracking endpoints
 * - Any other endpoints that shouldn't show loading indicator
 *
 * @param url - The request URL
 * @returns true if loading bar should be skipped
 */
function shouldSkipLoadingBar(url: string): boolean {
  const skipPatterns = [
    // Polling endpoints
    '/notifications/poll',
    '/messages/poll',

    // Health checks
    '/health',
    '/ping',
    '/status',

    // Analytics
    '/analytics',
    '/tracking',

    // Add more patterns as needed
  ];

  return skipPatterns.some(pattern => url.includes(pattern));
}
