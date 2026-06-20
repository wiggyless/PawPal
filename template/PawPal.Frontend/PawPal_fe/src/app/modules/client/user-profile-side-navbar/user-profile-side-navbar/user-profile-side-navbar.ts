import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogueComponent } from '../../dialogue-component/dialogue-component';
import { Router } from '@angular/router';
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
  ngOnInit(): void {
    console.log('SEL ITEM ->>>> ' + this.selItem);
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
    this.cd.detectChanges();
  }

  openDialog() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = { postDelete: false, profileDelete: true };
    this.dialog.open(DialogueComponent, dialogConfig);
  }
}
