import { ChangeDetectorRef, Component, DestroyRef, inject, signal } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { NavigationEnd, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { filter } from 'rxjs';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.scss',
})
export class AdminLayout {
  currentUser = inject(CurrentUserService);
  selectedItem: string = 'My_Profile';
  private router = inject(Router);
  cd = inject(ChangeDetectorRef);
  private destroyRef = inject(DestroyRef);
  hasLoaded: boolean = false;
  url = signal('');
  ngOnInit(): void {
    console.log(this.router.url);
    this.selectedItem = this.router.url;
  }
  constructor() {
    this.trackRouteChanges();
  }
  private trackRouteChanges(): void {
    this.router.events
      .pipe(
        // Filter out intermediate states (like NavigationStart, ResolveStart)
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        // Automatically unsubscribes when the component or service is destroyed
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((event: NavigationEnd) => {
        // Access the completed URL
        console.log('URL successfully changed to:', event.urlAfterRedirects);
        this.url.set(
          event.urlAfterRedirects.slice(
            event.urlAfterRedirects.indexOf('/', 1),
            event.urlAfterRedirects.length,
          ),
        );
        // Execute your custom logic here (e.g., analytics, resetting scroll position)
      });
  }
}
