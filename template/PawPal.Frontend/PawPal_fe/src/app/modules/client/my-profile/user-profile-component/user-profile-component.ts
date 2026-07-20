import { ChangeDetectorRef, Component, effect, inject, OnInit, signal } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { FormControl, FormGroup } from '@angular/forms';
import { UserService } from '../../../../api-services/users/users-service';
import { GetUserByIdDto, UpdateUserCommand } from '../../../../api-services/users/users-model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { forkJoin } from 'rxjs';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';
import { UserImageService } from '../../../../api-services/userImage/userImage-service';
import {
  UserImageCommand,
  UserImageQuery,
} from '../../../../api-services/userImage/userImage-model';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MatDialog } from '@angular/material/dialog';
import {
  CropDialogResult,
  UserProfileImageCropDialog,
} from './user-profile-imageCrop/user-profile-image-crop-dialog/user-profile-image-crop-dialog';
import { environment } from '../../../../../environments/environment';

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
  ngOnInit(): void {}
  ngOnDestroy(): void {
    if (this.objectUrl) {
      URL.revokeObjectURL(this.objectUrl);
    }
  }
  //services
  isUpdate: boolean = false;
  imageChanged = signal(false);
  currentUser: CurrentUserService;
  userDataService: UserService;
  dialog = inject(DialoguePopupService);
  dialogRef = inject(MatDialog);
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
    aboutMe: '',
    photoURL: '',
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
      username: '',
      aboutMe: '',
      photoURL: '',
    };
    this.profileForm.reset();
  }
  profileForm = new FormGroup({
    firstName: new FormControl({ value: '', disabled: true }),
    lastName: new FormControl({ value: '', disabled: true }),
    date: new FormControl({ value: '', disabled: true }),
    city: new FormControl<string | number>({ value: '', disabled: true }),
    aboutMe: new FormControl({ value: '', disabled: true }),
    username: new FormControl({ value: '', disabled: true }),
  });

  editing = signal(false);
  env = environment.apiUrl;
  getUserData(): void {
    this.userDataService.getUser(this.currentUser.userId() as number).subscribe((response) => {
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
      aboutMe: this.userData.aboutMe,
      username: this.userData.username,
    });
  }

  loadCities() {
    this.cityService.listCities().subscribe((res) => {
      this.cityList = res;
    });
  }

  editMode(id: number) {
    this.editing.set(true);
    this.loadCities();
    this.profileForm.enable();
    this.profileForm.get('city')?.setValue(this.originalCityId, { emitEvent: false });
  }
  saveChanges() {
    this.editing.set(false);

    const cityValue = this.profileForm.get('city')?.value;
    const cityId = typeof cityValue === 'number' ? cityValue : this.originalCityId;

    const firstName =
      (this.profileForm.get('firstName')?.value as string) || this.userData.firstName;
    const lastName = (this.profileForm.get('lastName')?.value as string) || this.userData.lastName;
    const aboutMe =
      (this.profileForm.get('aboutMe')?.value as string) || this.userData.aboutMe || '';
    const date = (this.profileForm.get('date')?.value as string) || this.userData.dateTime;

    const formChanged = this.hasFormFieldChanges(cityId);
    const imageChanged = this.hasImageChanges();

    this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });

    if (!formChanged && !imageChanged) {
      return;
    }

    const requests: any = {};

    if (formChanged) {
      const payload: UpdateUserCommand = {
        firstName: firstName,
        lastName: lastName,
        profilePictureURL: this.imageUrl()?.toString() ?? '',
        date: date,
        cityId: cityId,
        aboutMe: aboutMe,
      };
      requests.userPostData = this.userDataService.updateUser(this.userData.id, payload);
    }
    if (imageChanged) {
      requests.userImage = this.isUpdate
        ? this.userImageService.updateUserImage(this.userData.id, this.selectedImage!)
        : this.userImageService.createUserImage(this.userData.id, this.selectedImage!);
    }

    forkJoin(requests).subscribe({
      next: (res) => {
        this.userData.firstName = firstName;
        this.userData.lastName = lastName;
        this.userData.dateTime = date;
        this.userData.aboutMe = aboutMe;

        const selectedCity = this.cityList.items.find((city: any) => city.id === cityId);
        if (selectedCity) {
          this.userData.city = selectedCity.name;
          this.originalCityId = cityId;
        }
        this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });

        this.dialog.success('Success', 'Your profile has been updated successfully.', 'OK');
      },
      error: (res) => {
        this.dialog.error(
          'Error',
          'An error occurred while updating your profile. Please try again.',
          'OK',
        );
      },
    });
    this.profileForm.disable();
  }
  hasFormFieldChanges(cityId: number): boolean {
    const formDate = this.profileForm.get('date')?.value;
    const originalDate = this.userData.dateTime;

    const normalizeDate = (val: any): string => {
      if (!val) return '';
      const d = new Date(val);
      return isNaN(d.getTime()) ? String(val) : d.toISOString().split('T')[0];
    };

    return (
      this.userData.firstName !== this.profileForm.get('firstName')?.value ||
      this.userData.lastName !== this.profileForm.get('lastName')?.value ||
      normalizeDate(originalDate) !== normalizeDate(formDate) ||
      cityId !== this.originalCityId ||
      this.userData.aboutMe !== this.profileForm.get('aboutMe')?.value ||
      this.userData.username !== this.profileForm.get('username')?.value
    );
  }

  hasImageChanges(): boolean {
    return this.imageChanged() && !!this.selectedImage;
  }

  checkFormChanges(): boolean {
    return this.hasFormFieldChanges(this.originalCityId) || this.hasImageChanges();
  }

  editPhoto(event: any): void {
    const files = event.target.files;
    if (!files || files.length === 0) return;

    const file: File = files[0];

    event.target.value = '';
    this.dialogRef
      .open(UserProfileImageCropDialog, {
        data: { imageFile: file },
        width: '40vw',
        maxWidth: '40vw',
        maxHeight: '95vh',
        disableClose: true,
        panelClass: 'image-crop-dialog-panel',
      })
      .afterClosed()
      .subscribe((result: CropDialogResult | undefined) => {
        if (!result) return;

        this.selectedImage = result.croppedFile;
        this.imageChanged.set(true);
        this.imageUrl.set(result.croppedUrl);
      });
  }
  cancelSaving() {
    this.editing.set(false);
    this.imageChanged.set(false);
    this.selectedImage = undefined;
    this.profileForm.disable();
    this.imageUrl.set(this.originalImageUrl);
    this.profileForm.get('city')?.setValue(this.userData.city, { emitEvent: false });
    this.profileForm.get('aboutMe')?.setValue(this.userData.aboutMe ?? '', { emitEvent: false });
  }
}
