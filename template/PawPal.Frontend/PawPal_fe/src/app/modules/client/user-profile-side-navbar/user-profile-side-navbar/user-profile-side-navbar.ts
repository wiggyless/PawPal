import {
  ChangeDetectorRef,
  Component,
  DestroyRef,
  inject,
  Input,
  OnInit,
  signal,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogueComponent } from '../../dialogue-component/dialogue-component';
import { NavigationEnd, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { filter } from 'rxjs';
@Component({
  selector: 'app-user-profile-side-navbar',
  standalone: false,
  templateUrl: './user-profile-side-navbar.html',
  styleUrl: './user-profile-side-navbar.scss',
})
export class UserProfileSideNavbar implements OnInit {
  @Input() selItem: string | null = null;
  dialog = inject(MatDialog);
  private router = inject(Router);
  hasLoaded = false;
  cd = inject(ChangeDetectorRef);
  destroyRef = inject(DestroyRef);
  url = signal('');
  ngOnInit(): void {
    this.url.set(this.selItem as string);
    this.trackRouteChanges();
  }
  matListItems = {
    My_Profile: '/client/my-profile',
    My_Favorites: '/client/my-favorties',
    My_Posts: '/client/myPosts',
    Recieved_Requests: '/client/my-requests',
    Sent_Requests: '/client/my-sent-requests',
    Request_History: '/client/request-history',
    Settings: '/client/settings',
  };
  keepOrder = (a: any, b: any) => 0;
  onSelect(item: string) {
    this.selItem = item;
    this.url.set(this.selItem);
    this.cd.detectChanges();
  }
  private trackRouteChanges(): void {
    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((event: NavigationEnd) => {
        this.url.set(event.urlAfterRedirects);
        this.cd.detectChanges();
      });
  }
  openDialog() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = { postDelete: false, profileDelete: true };
    this.dialog.open(DialogueComponent, dialogConfig);
  }
}
