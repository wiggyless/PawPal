import { ChangeDetectorRef, Component, effect, inject, OnInit, signal } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, Subscriber, Subscription } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';
import { ReportUserService } from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.service';
import { UserImageCommand } from '../../../../api-services/userImage/userImage-model';
import { UserImageService } from '../../../../api-services/userImage/userImage-service';
import { UserDisabledService } from '../../../../api-services/users-disabled/users-disabled.service';
import { GetUserByIdDto } from '../../../../api-services/users/users-model';
import { UserService } from '../../../../api-services/users/users-service';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { ProfileDisableDialog } from '../../../admin/user-profiles/user-profiles/profile/profile/profile-disable-dialog/profile-disable-dialog/profile-disable-dialog';
import { ReportUserComponent } from '../../../client/report-user-component/report-user-component/report-user-component';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { PageResult } from '../../../../core/models/paging/page-result';
import { ListAnimal } from '../../../../api-services/animal-posts/animal-posts.model';
import { PageEvent } from '@angular/material/paginator';

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
  postService = inject(AnimalPostService);
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
  userID: number | undefined;
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
  paging = { page: 1, pageSize: 4 };
  postList: PageResult<ListAnimal> | undefined;
  isLoaded = signal(false);
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
      userData: this.userDataService.getUser(this.userID as number),
      cities: this.cityService.listCities(),
    }).subscribe({
      next: (response) => {
        this.userData = response.userData;
        this.loadPosts();
        this.cityList = response.cities;
      },
    });
  }
  loadPosts() {
    this.postService
      .listAnimalPostsByUserId({ userID: this.userData.id, paging: this.paging })
      .subscribe({
        next: (res) => {
          this.initializeInputData();
          this.postList = res;
          this.isLoaded.set(true);
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
      this.cityList = res;
    });
  }

  reportProfile(): void {
    this.dialog.open(ReportUserComponent, {
      data: { reportedUserID: this.userID, reportSentByID: this.currentUser.userId() },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.paging.page = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.loadPosts();
  }
  routeToPost(post: ListAnimal) {
    this.route.navigate(['post'], {
      queryParams: {
        postID: post.postID,
      },
    });
  }
}
