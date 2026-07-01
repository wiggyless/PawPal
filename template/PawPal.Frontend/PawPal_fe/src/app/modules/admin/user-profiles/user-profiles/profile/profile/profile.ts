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
import { ActivatedRoute, Router } from '@angular/router';
import { UserDisabledService } from '../../../../../../api-services/users-disabled/users-disabled.service';
import { ProfileDisableDialog } from './profile-disable-dialog/profile-disable-dialog/profile-disable-dialog';
import { environment } from '../../../../../../../environments/environment.development';
import { ReportUserService } from '../../../../../../api-services/moderation/reported-posts/reported-users/reported-users.service';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile implements OnInit {
  //services
  isUpdate: boolean = false;
  currentUser = inject(CurrentUserService);
  userDataService = inject(UserService);
  dialog = inject(MatDialog);
  dialogRef = inject(MatDialog);
  cityService = inject(CitiesService);
  userImageService = inject(UserImageService);
  cd = inject(ChangeDetectorRef);
  router = inject(ActivatedRoute);
  userDisabledService = inject(UserDisabledService);
  dialogPopUp = inject(DialoguePopupService);
  userReportsService = inject(ReportUserService);
  cityList: any = [];
  city: string = '';
  selectedImage: File | undefined;
  objectUrl: string | null = null;
  private originalCityId: number = 0;
  imageUrl = signal<SafeUrl | null>(null);
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
    aboutMe: '',
    photoURL: '',
  };
  userImage: UserImageCommand = {
    userID: 0,
  };
  private userID: number | undefined;
  private repID: number | undefined;
  route = inject(Router);
  profileForm = new FormGroup({
    firstName: new FormControl({ value: '', disabled: true }),
    lastName: new FormControl({ value: '', disabled: true }),
    date: new FormControl({ value: '', disabled: true }),
    city: new FormControl<string | number>({ value: '', disabled: true }),
    aboutMe: new FormControl({ value: '', disabled: true }),
  });
  env = environment.apiUrl;
  sub: Subscription | undefined;
  editing: boolean = false;
  disabled: boolean | undefined;
  ngOnInit(): void {
    this.router.queryParams.subscribe((params) => {
      this.userID = params['userID'];
      this.repID = params['repID'];
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
      userData: this.userDataService.getUserDisabled(this.userID as number),
      cities: this.cityService.listCities(),
    }).subscribe({
      next: (response) => {
        this.userData = response.userData;
        this.cityList = response.cities;
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
  disableAccount() {
    this.dialogPopUp.warning(
      'Confirm action',
      'Are you sure you want to disable ' + this.userData.username + ' profile?',
      'YES',
      'NO',
      () => {
        this.dialog
          .open(ProfileDisableDialog, {
            data: {
              userId: this.userData.id,
            },
          })
          .afterClosed()
          .subscribe((res) => {
            this.userReportsService.deleteUserReport(this.repID as number).subscribe((res) => {
              this.route.navigate(['reported-users']);
            });
          });
      },
    );
  }
  activateAccount() {
    this.dialogPopUp.warning(
      'Confirm action',
      'Are you sure you want to activate ' + this.userData.username + ' profile?',
      'YES',
      'NO',
      () => {
        this.userDisabledService.deleteUserDisable(this.userData.id).subscribe({
          next: (res) => {
            this.dialogPopUp.success('Success', 'User profile has been activated', 'OK');
            this.route.navigate(['disabled-users']);
          },
          error: (res) => {
            this.dialogPopUp.error('Error', 'An error has occured', 'OK');
          },
        });
      },
    );
  }
}
