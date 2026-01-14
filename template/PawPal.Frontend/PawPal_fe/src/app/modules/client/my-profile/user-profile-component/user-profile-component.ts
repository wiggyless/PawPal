import { Component, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { FormControl, FormGroup } from '@angular/forms';
import { AnimalUserService } from '../../../../api-services/animal-users/animal-users-service';
import { GetUserByIdDto } from '../../../../api-services/animal-users/animal-users-model';

@Component({
  selector: 'app-user-profile-component',
  standalone: false,
  templateUrl: './user-profile-component.html',
  styleUrl: './user-profile-component.scss',
})
export class UserProfileComponent implements OnInit {
  ngOnInit(): void {
    console.log(this.currentUser.userId);
    this.getUserData();
  }
  currentUser: CurrentUserService;
  userDataService: AnimalUserService;
  userData: GetUserByIdDto = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    dateTime: '',
    city: '',
    cantonAbbrevation: '',
    cityID: 0,
  };
  constructor(currentUser: CurrentUserService, userDataService: AnimalUserService) {
    this.currentUser = currentUser;
    this.userDataService = userDataService;
  }
  profileForm = new FormGroup({
    firstName: new FormControl({ value: '', disabled: true }),
    lastName: new FormControl({ value: '', disabled: true }),
    date: new FormControl({ value: '', disabled: true }),
    city: new FormControl({ value: '', disabled: true }),
    aboutMe: new FormControl({ value: '', disabled: true }),
  });
  getUserData(): void {
    this.userDataService.getUser(this.currentUser.userId).subscribe((response) => {
      this.userData = response;
      this.initializeInputData();
    });
  }
  initializeInputData(): void {
    this.profileForm.patchValue({
      firstName: this.userData.firstName,
      lastName: this.userData.lastName,
      date: this.userData.dateTime,
      city: this.userData.city + ', ' + this.userData.cantonAbbrevation,
    });
  }
}
