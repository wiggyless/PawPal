import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-user-profile-side-navbar',
  standalone: false,
  templateUrl: './user-profile-side-navbar.html',
  styleUrl: './user-profile-side-navbar.scss',
})
export class UserProfileSideNavbar implements OnInit {
  selectedItem: string = 'My_Profile';
  ngOnInit(): void {
    this.selectedItem = history.state.item == undefined ? 'My_Profile' : history.state.item;
  }
  matListItems = {
    My_Profile: '/client/my-profile',
    My_Favorites: '',
    My_Posts: '/client/my-profile/myPosts',
    My_Requests: '',
    My_Messages: '',
    My_Animals: '',
    Settings: '',
  };
  keepOrder = (a: any, b: any) => 0;
  onSelect(item: string) {
    this.selectedItem = item;
  }
}
