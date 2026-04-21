import { Component, inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogueComponent } from '../../dialogue-component/dialogue-component';
@Component({
  selector: 'app-user-profile-side-navbar',
  standalone: false,
  templateUrl: './user-profile-side-navbar.html',
  styleUrl: './user-profile-side-navbar.scss',
})
export class UserProfileSideNavbar implements OnInit {
  selectedItem: string = 'My_Profile';
  dialog = inject(MatDialog);
  ngOnInit(): void {
    this.selectedItem = history.state.item == undefined ? 'My_Profile' : history.state.item;
  }
  matListItems = {
    My_Profile: '/client/my-profile',
    My_Favorites: '/client/my-profile/my-favorties',
    My_Posts: '/client/my-profile/myPosts',
    Recieved_Requests: '/client/my-profile/my-requests',
    Sent_Requests: '/client/my-profile/my-sent-requests',
    Request_History: '/client/my-profile/request-history',
    Settings: '/client/my-profile/settings',
  };
  keepOrder = (a: any, b: any) => 0;
  onSelect(item: string) {
    this.selectedItem = item;
  }

  openDialog() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = { postDelete: false, profileDelete: true };
    this.dialog.open(DialogueComponent, dialogConfig);
  }
}
