import { Component, effect, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { FormControl, FormGroup } from '@angular/forms';
import { UserService } from '../../../../api-services/users/users-service';
import {
  GetUserByIdDto,
  UpdateUserCommand,
} from '../../../../api-services/users/users-model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { forkJoin } from 'rxjs';
import { DialoguePopupService } from '../../../shared/components/dialogue-popup/dialogue-popup.service';

@Component({
  selector: 'app-user-profile-component',
  standalone: false,
  templateUrl: './user-profile-component.html',
  styleUrl: './user-profile-component.scss',
})
export class UserProfileComponent implements OnInit {
  constructor(currentUser: CurrentUserService, userDataService: UserService) {
    this.currentUser = currentUser;
    this.userDataService = userDataService;
     effect(() => {
      const userId = this.currentUser.userId();
      if (!userId) {
        this.resetUserData();
        return;
      }
      forkJoin({
        userData: this.userDataService.getUser(userId),
        cities: this.cityService.listCities(),
      }).subscribe({
        next: (response) => {
          this.userData = response.userData;
          this.initializeInputData();
        },
      });
    });
  }

  ngOnInit(): void {
  }
  currentUser: CurrentUserService;
  userDataService: UserService;
  dialog = inject(DialoguePopupService);
  cityService = inject(CitiesService);
  cityList: any = [];
  city: string = '';

  private originalCityId: number = 0;

  userData: GetUserByIdDto = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    dateTime: '',
    city: '',
    cantonAbbrevation: '',
    cityID: 0,
    userName: '',
  };

  private resetUserData(): void {
    this.userData = {
      id: 0, firstName: '', lastName: '', email: '',
      dateTime: '', city: '', cantonAbbrevation: '', cityID: 0, userName: '',
    };
    this.profileForm.reset();
  }

  profileForm = new FormGroup({
    firstName: new FormControl({ value: '', disabled: true }),
    lastName: new FormControl({ value: '', disabled: true }),
    date: new FormControl({ value: '', disabled: true }),
    city: new FormControl<string | number>({ value: '', disabled: true }), // Accept both types
    aboutMe: new FormControl({ value: '', disabled: true }),
  });

  editing: boolean = false;

  getUserData(): void {
    this.userDataService.getUser(this.currentUser.userId()).subscribe((response) => {
      this.userData = response;
      this.initializeInputData();
    });
  }
  initializeInputData(): void {
    this.originalCityId = this.userData.cityID;

    this.profileForm.patchValue({
      firstName: this.userData.firstName,
      lastName: this.userData.lastName,
      date: this.userData.dateTime,
      city: this.userData.city,
    });
  }

  loadCities() {
    this.cityService.listCities().subscribe((res) => {
      console.log(res);
      this.cityList = res;
    });
  }

  editMode(id: number) {
    this.editing = true;
    this.loadCities();
    this.profileForm.enable();
    this.profileForm.get('city')?.setValue(this.originalCityId, { emitEvent: false });
  }

  saveChanges() {
    this.editing = false;
    this.dialog.open(SaveChangesComponent);
    const cityValue = this.profileForm.get('city')?.value;

    const cityId = typeof cityValue === 'number' ? cityValue : this.originalCityId;

    this.userData.firstName = this.profileForm.get('firstName')?.value as string;
    this.userData.lastName = this.profileForm.get('lastName')?.value as string;
    this.userData.dateTime = this.profileForm.get('date')?.value as string;
    this.userData.cityID = cityId;

    const payload: UpdateUserCommand = {
      firstName: this.userData.firstName,
      lastName: this.userData.lastName,
      profilePictureURL: '',
      date: this.userData.dateTime,
      cityId: cityId,
    };

    this.userDataService.updateUser(this.userData.id, payload).subscribe({
      next: (res) => {
        const selectedCity = this.cityList.items.find((city: any) => city.id === cityId);

        if (selectedCity) {
          this.userData.city = selectedCity.name;
          this.originalCityId = cityId;
        }
        this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
        this.dialog.success('Success', 'Your profile has been updated successfully.', 'OK');
      },
      error: (res) => {
        console.log('ERROR: =>', res);
        this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
        this.dialog.error('Error', 'An error occurred while updating your profile. Please try again.', 'OK');
      },
    });

    this.profileForm.disable();
  }

  editPhoto() {}
}
