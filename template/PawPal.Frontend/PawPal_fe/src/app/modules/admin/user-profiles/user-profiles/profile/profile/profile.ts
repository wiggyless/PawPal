import { ChangeDetectorRef, Component, effect, inject, OnInit, signal } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { forkJoin, Subscriber, Subscription } from 'rxjs';
import { CitiesService } from '../../../../../../api-services/cities/cities.service';
import {
  UserImageQuery,
  UserImageCommand,
} from '../../../../../../api-services/userImage/userImage-model';
import { UserImageService } from '../../../../../../api-services/userImage/userImage-service';
import {
  GetUserByIdDto,
  UpdateUserCommand,
} from '../../../../../../api-services/users/users-model';
import { UserService } from '../../../../../../api-services/users/users-service';
import { CurrentUserService } from '../../../../../../core/services/auth/current-user.service';
import {
  UserProfileImageCropDialog,
  CropDialogResult,
} from '../../../../../client/my-profile/user-profile-component/user-profile-imageCrop/user-profile-image-crop-dialog/user-profile-image-crop-dialog';
import { DialoguePopupService } from '../../../../../../api-services/dialogue-popup/dialogue-popup.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile implements OnInit {
  //services
  isUpdate: boolean = false;
  private imageChanged: boolean = false;
  currentUser = inject(CurrentUserService);
  userDataService = inject(UserService);
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
  originalUrl: string | null = null;
  router = inject(ActivatedRoute);
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
  };
  userImage: UserImageCommand = {
    userID: 0,
  };
  private userID: number | undefined;
  profileForm = new FormGroup({
    firstName: new FormControl({ value: '', disabled: true }),
    lastName: new FormControl({ value: '', disabled: true }),
    date: new FormControl({ value: '', disabled: true }),
    city: new FormControl<string | number>({ value: '', disabled: true }),
    aboutMe: new FormControl({ value: '', disabled: true }),
  });
  sub: Subscription | undefined;
  editing: boolean = false;
  ngOnInit(): void {
    this.router.queryParams.subscribe((params) => {
      this.userID = params['userID'];
      this.getUserData();
    });
  }
  ngOnDestroy(): void {
    if (this.objectUrl) {
      URL.revokeObjectURL(this.objectUrl);
    }
    this.sub?.unsubscribe();
  }
  getUserData(): void {
    this.sub = forkJoin({
      userData: this.userDataService.getUser(this.userID),
      cities: this.cityService.listCities(),
    }).subscribe({
      next: (response) => {
        this.userData = response.userData;
        this.cityList = response.cities;
        this.userImageService.getUserImageByID(this.userID!).subscribe({
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
  }
  initializeInputData(): void {
    this.originalCityId = this.userData.cityID;
    this.profileForm.patchValue({
      firstName: this.userData.firstName,
      lastName: this.userData.lastName,
      date: this.userData.dateTime,
      city: this.userData.city,
      aboutMe: this.userData.aboutMe,
    });
  }

  loadCities() {
    this.cityService.listCities().subscribe((res) => {
      console.log(res);
      this.cityList = res;
    });
  }
}
