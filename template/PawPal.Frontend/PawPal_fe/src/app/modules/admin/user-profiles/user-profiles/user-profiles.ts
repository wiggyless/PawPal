import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { UserService } from '../../../../api-services/users/users-service';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { Observable, Subscriber, Subscription, tap } from 'rxjs';
import { environment } from '../../../../../environments/environment.development';
import { GetUserList, GetUserQuery } from '../../../../api-services/users/users-model';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { PageResult } from '../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { UserImageService } from '../../../../api-services/userImage/userImage-service';

@Component({
  selector: 'app-user-profiles',
  standalone: false,
  templateUrl: './user-profiles.html',
  styleUrl: './user-profiles.scss',
})
export class UserProfiles
  extends BaseListPagedComponent<GetUserList, GetUserQuery>
  implements OnInit, OnDestroy
{
  currentUser: any;
  userProfileService = inject(UserService);
  userImgService = inject(UserImageService);
  userProfilesList: PageResult<GetUserList> | undefined;
  envLink = environment;
  isLoaded = signal(false);
  route = inject(Router);
  counter: number = 0;
  isLoadingMore = signal(false);
  subscribe: Subscription | undefined;
  cd = inject(ChangeDetectorRef);
  constructor(crr: CurrentUserService) {
    super();
    this.currentUser = crr;
    this.request = new GetUserQuery();
    this.request.paging.pageSize = 4;
  }
  env = environment.apiUrl;
  protected override loadPagedData(): void {}
  // Page Values ( didnt use the template cuz whats going on???)
  page = {
    pageSize: 10,
    currentPage: 1,
    includedTotal: true,
    totalItems: 5,
    totalPages: 0,
    pageSizeOption: [4, 8],
  };
  username = '';
  ngOnInit(): void {
    this.loadUsers();
  }
  ngOnDestroy(): void {
    this.subscribe?.unsubscribe();
  }
  loadUsers(): void {
    this.request.searchUsername = this.username;
    this.isLoaded.set(false);
    this.subscribe = this.userProfileService.getUserList(this.request).subscribe((res) => {
      this.userProfilesList = res;
      this.isLoaded.set(true);
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadUsers();
  }
  routeToProfile(id: number) {
    this.route.navigate(['/admin/profile'], {
      queryParams: {
        userID: id,
      },
    });
  }
}
