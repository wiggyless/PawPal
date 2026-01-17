import { Component, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { FormControl, FormGroup } from '@angular/forms';
import { AnimalUserService } from '../../../../api-services/animal-users/animal-users-service';
import { GetUserByIdDto, UpdateUserCommand } from '../../../../api-services/animal-users/animal-users-model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
@Component({
  selector: 'app-user-profile-component',
  standalone: false,
  templateUrl: './user-profile-component.html',
  styleUrl: './user-profile-component.scss',
})
export class UserProfileComponent implements OnInit {
  ngOnInit(): void {
    this.getUserData();
  }
  currentUser: CurrentUserService;
  userDataService: AnimalUserService;
  cityService = inject(CitiesService);
  cityList : any = [];
  city : string = '';
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

  editing : boolean = false;

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
      city: this.userData.city,
    });
  }

  loadCities() {
    this.cityService.listCities().subscribe(res=>{
      console.log(res);
      this.cityList = res;
    })
  }

  editMode(id: number){
    this.editing = true;
    this.loadCities();
    this.profileForm.enable();
  }

  saveChanges(){
    this.editing = false;
    this.userData.firstName = this.profileForm.get('firstName')?.value as string;
     this.userData.lastName  = this.profileForm.get('lastName')?.value as string;
     this.userData.dateTime = this.profileForm.get('date')?.value as string;
    this.userData.cityID = this.profileForm.get('city')?.value as any;

     const selectedCity = this.cityList.items?.find((city: any) => city.id === this.userData.cityID);
  if (selectedCity) {
    this.userData.city = selectedCity.name;
  }
    const payload: UpdateUserCommand = {
      firstName: this.userData.firstName,
      lastName: this.userData.lastName,
      profilePictureURL : '',
      date : this.userData.dateTime,
      cityId : this.userData.cityID
    };

    this.userDataService.updateUser(this.userData.id, payload).subscribe({
      next: (res) => {
          console.log('SUCCESS =>', res);
          this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
      },
      error: (res)=>{
        console.log('ERROR: =>', res);
      }
    })
    this.profileForm.disable();
  }
}
