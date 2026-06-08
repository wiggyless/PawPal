import { ChangeDetectorRef, Component, effect, inject, OnInit, signal } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { FormControl, FormGroup } from '@angular/forms';
import { UserService } from '../../../../api-services/users/users-service';
import { GetUserByIdDto, UpdateUserCommand } from '../../../../api-services/users/users-model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { forkJoin } from 'rxjs';
import { DialoguePopupService } from '../../../shared/components/dialogue-popup/dialogue-popup.service';
import { UserImageService } from '../../../../api-services/userImage/userImage-service';
import {
  UserImageCommand,
  UserImageQuery,
} from '../../../../api-services/userImage/userImage-model';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { FileSelectEvent } from 'primeng/types/fileupload';

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
      const usrImg: UserImageQuery = {
        userID: userId!,
      };
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
          this.cityList = response.cities;
          this.userImageService.getUserImageByID(this.userData.id).subscribe({
            next: (res) => {
              this.isUpdate = true;
              this.imageUrl.set(res);
              this.originalImageUrl = res;
            },
            error: () => {
              this.isUpdate = false;
              this.imageUrl.set(null);
              this.originalImageUrl = null;
            },
          });
          this.initializeInputData();
        },
      });
    });
  }
  ngOnInit(): void {
    this.userData;
  }
  ngOnDestroy(): void {
    if (this.objectUrl) {
      URL.revokeObjectURL(this.objectUrl);
    }
  }
  //services
  isUpdate: boolean = false;
  private imageChanged: boolean = false;
  currentUser: CurrentUserService;
  userDataService: UserService;
  dialog = inject(DialoguePopupService);
  cityService = inject(CitiesService);
  userImageService = inject(UserImageService);
  cd = inject(ChangeDetectorRef);
  cityList: any = [];
  city: string = '';
  selectedImage: File | undefined;
  objectUrl: string | null = null;
  private originalCityId: number = 0;
  private sanitizer = inject(DomSanitizer);
  imageUrl = signal<SafeUrl | null>(null);
  private originalImageUrl: SafeUrl | null = null;
  originalUrl: string | null = null;
  userData: GetUserByIdDto = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    dateTime: '',
    city: '',
    cantonAbbrevation: '',
    cityID: 0,
    username: '',
  };
  userImage: UserImageCommand = {
    userID: 0,
  };
  private resetUserData(): void {
    this.userData = {
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
      city: this.userData.city
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
    this.profileForm.disable();

    const formChanged = this.hasFormFieldChanges();
    const imageChanged = this.hasImageChanges();

    if (!formChanged && !imageChanged) {
      // Nothing changed, skip all API calls
      return;
    }

    const cityValue = this.profileForm.get('city')?.value;
    const cityId = typeof cityValue === 'number' ? cityValue : this.originalCityId;

    const requests: any = {};

    if (formChanged) {
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
      requests.userPostData = this.userDataService.updateUser(this.userData.id, payload);
    }

    if (imageChanged) {
      console.log('WORKS');
      requests.userImage = this.isUpdate
        ? this.userImageService.updateUserImage(this.userData.id, this.selectedImage!)
        : this.userImageService.createUserImage(this.userData.id, this.selectedImage!);
    }

    forkJoin(requests).subscribe({
      next: (res) => {
        if (formChanged) {
          const selectedCity = this.cityList.items.find((city: any) => city.id === cityId);
          if (selectedCity) {
            this.userData.city = selectedCity.name;
            this.originalCityId = cityId;
          }
          this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
        }
        if (imageChanged) {
          this.isUpdate = true;
          this.imageChanged = false;
          this.originalImageUrl = this.imageUrl();
          this.selectedImage = undefined;
        }
        this.dialog.success('Success', 'Your profile has been updated successfully.', 'OK');
      },
      error: (res) => {
        console.log('ERROR: =>', res);
        this.dialog.error(
          'Error',
          'An error occurred while updating your profile. Please try again.',
          'OK',
        );
      },
    });
  }
  // need to fix updating, only to update if a change has happened
  /* saveChanges() {
    this.editing = false;
    //this.dialog.open(SaveChangesComponent);
    if (this.checkFormChanges()) {
      const cityValue = this.profileForm.get('city')?.value;
      const cityId = typeof cityValue === 'number' ? cityValue : this.originalCityId;
      this.userData.firstName = this.profileForm.get('firstName')?.value as string;
      this.userData.lastName = this.profileForm.get('lastName')?.value as string;
      this.userData.dateTime = this.profileForm.get('date')?.value as string;
      this.userData.cityID = cityId;
      const payload: UpdateUserCommand = {
        firstName: this.userData.firstName,
        lastName: this.userData.lastName,
        profilePictureURL: '', // needs to be removed
        date: this.userData.dateTime,
        cityId: cityId,
      };
      forkJoin({
        userPostData: this.userDataService.updateUser(this.userData.id, payload),
        userImage:
          this.isUpdate == false
            ? this.userImageService.createUserImage(this.userData.id, this.selectedImage!)
            : this.userImageService.updateUserImage(this.userData.id, this.selectedImage!),
      }).subscribe({
        next: (res) => {
          const selectedCity = this.cityList.items.find((city: any) => city.id === cityId);
          if (selectedCity) {
            this.userData.city = selectedCity.name;
            this.originalCityId = cityId;
          }
          this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
          this.dialog.success('Success', 'Your profile has been updated successfully.', 'OK');
          window.location.reload();
        },
        error: (res) => {
          console.log('ERROR: =>', res);
          this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
          this.dialog.error(
            'Error',
            'An error occurred while updating your profile. Please try again.',
            'OK',
          );
        },
      });
    }
    /*
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
        this.dialog.error(
          'Error',
          'An error occurred while updating your profile. Please try again.',
          'OK',
        );
      },
    });
    this.profileForm.disable();
  }
    */
  hasFormFieldChanges(): boolean {
    return (
      this.userData.firstName !== this.profileForm.get('firstName')?.value ||
      this.userData.lastName !== this.profileForm.get('lastName')?.value ||
      this.userData.dateTime !== this.profileForm.get('date')?.value ||
      this.originalCityId !== this.profileForm.get('city')?.value
    );
  }

  hasImageChanges(): boolean {
    return this.imageChanged && !!this.selectedImage;
  }

  checkFormChanges(): boolean {
    return this.hasFormFieldChanges() || this.hasImageChanges();
  }
  editPhoto(event: any) {
    const files = event.target.files;
    if (files && files.length > 0) {
      const file = files[0];
      this.selectedImage = file;
      this.imageChanged = true;
      const reader = new FileReader();
      reader.onload = () => {
        this.originalUrl = this.imageUrl()?.toString() ?? null;
        this.imageUrl.set(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  }
  cancelSaving() {
    this.editing = false;
    this.imageChanged = false;
    this.selectedImage = undefined;
    this.profileForm.disable();
    this.imageUrl.set(this.originalImageUrl);
  }
}
