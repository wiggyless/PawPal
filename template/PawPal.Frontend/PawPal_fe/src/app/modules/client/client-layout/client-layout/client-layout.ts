import { ChangeDetectorRef, Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { Router, NavigationEnd, Event } from '@angular/router';
import { filter } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
@Component({
  selector: 'app-client-layout',
  standalone: false,
  templateUrl: './client-layout.html',
  styleUrls: ['./client-layout.scss'],
})
export class ClientLayout implements OnInit {
  currentUser: any;
  selectedItem: string = 'My_Profile';
  private router = inject(Router);
  cd = inject(ChangeDetectorRef);
  private destroyRef = inject(DestroyRef);
  constructor(crr: CurrentUserService) {
    this.currentUser = crr;
  }
  hasLoaded: boolean = false;
  url = signal('');
  ngOnInit(): void {
    this.selectedItem = this.router.url;
    console.log(this.selectedItem);
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
