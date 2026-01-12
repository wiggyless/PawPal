import { Component, OnInit } from '@angular/core';
import { inject } from '@angular/core/primitives/di';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-user-profile-component',
  standalone: false,
  templateUrl: './user-profile-component.html',
  styleUrl: './user-profile-component.scss',
})
export class UserProfileComponent implements OnInit {
  currentUser: CurrentUserService;
  constructor(currentUser: CurrentUserService) {
    this.currentUser = currentUser;
  }
  ngOnInit(): void {
    console.log(this.currentUser.roleid);
  }
}
