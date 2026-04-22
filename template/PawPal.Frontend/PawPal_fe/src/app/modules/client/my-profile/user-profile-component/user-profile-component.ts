import { Component, effect, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { FormControl, FormGroup } from '@angular/forms';
import { AnimalUserService } from '../../../../api-services/animal-users/animal-users-service';
import {
  GetUserByIdDto,
  UpdateUserCommand,
} from '../../../../api-services/animal-users/animal-users-model';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { forkJoin } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { SaveChangesComponent } from './save-changes-component/save-changes-component';

@Component({
  selector: 'app-user-profile-component',
  standalone: false,
  templateUrl: './user-profile-component.html',
  styleUrl: './user-profile-component.scss',
})
export class UserProfileComponent implements OnInit {
  constructor(currentUser: CurrentUserService, userDataService: AnimalUserService) {
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
  userDataService: AnimalUserService;
  dialog = inject(MatDialog);
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
    this.originalCityId = this.userData.cityID; // Store the original ID

    this.profileForm.patchValue({
      firstName: this.userData.firstName,
      lastName: this.userData.lastName,
      date: this.userData.dateTime,
      city: this.userData.city, // This is the city name (string) for display
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
    // Get the city value from the form
    const cityValue = this.profileForm.get('city')?.value;

    // Determine the cityId: use form value if it's a number, otherwise use original
    const cityId = typeof cityValue === 'number' ? cityValue : this.originalCityId;

    // Collect form data
    this.userData.firstName = this.profileForm.get('firstName')?.value as string;
    this.userData.lastName = this.profileForm.get('lastName')?.value as string;
    this.userData.dateTime = this.profileForm.get('date')?.value as string;
    this.userData.cityID = cityId;

    // Create payload
    const payload: UpdateUserCommand = {
      firstName: this.userData.firstName,
      lastName: this.userData.lastName,
      profilePictureURL: '',
      date: this.userData.dateTime,
      cityId: cityId,
    };

    this.userDataService.updateUser(this.userData.id, payload).subscribe({
      next: (res) => {
        // Find the selected city name to display - with explicit type
        const selectedCity = this.cityList.items.find((city: any) => city.id === cityId);

        if (selectedCity) {
          this.userData.city = selectedCity.name;
          this.originalCityId = cityId; // Update the original ID
        }

        // Set the form back to show city name (not ID)
        this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
      },
      error: (res) => {
        console.log('ERROR: =>', res);
        // Reset to original city name on error
        this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
      },
    });

    this.profileForm.disable();
  }
}
