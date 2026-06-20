import { Component, inject, OnInit, signal } from '@angular/core';
import { UserService } from '../../../../api-services/users/users-service';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../../../environments/environment.development';
import {
  ListAnimal,
  GetPostQuery,
  ListAnimalPostsByUserIdDto,
} from '../../../../api-services/animal-posts/animal-posts.model';
import { GetUserList, GetUserQuery } from '../../../../api-services/users/users-model';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { PageResult } from '../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-user-profiles',
  standalone: false,
  templateUrl: './user-profiles.html',
  styleUrl: './user-profiles.scss',
})
export class UserProfiles
  extends BaseListPagedComponent<GetUserList, GetUserQuery>
  implements OnInit
{
  currentUser: any;
  userProfileService = inject(UserService);
  userProfilesList: Observable<PageResult<GetUserList>> | undefined;
  envLink = environment;
  isLoaded = signal(false);
  route = inject(Router);
  constructor(crr: CurrentUserService) {
    super();
    this.currentUser = crr;
    this.request = new GetUserQuery();
    this.request.paging.pageSize = 4;
  }
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

  ngOnInit(): void {
    console.log('USERS LOADED ');
    this.loadUsers();
  }
  loadUsers(): void {
    this.userProfilesList = this.userProfileService.getUserList(this.request).pipe(
      tap((res) => {
        this.page = {
          pageSize: res.pageSize,
          currentPage: res.currentPage,
          includedTotal: res.includedTotal,
          totalItems: res.totalItems,
          totalPages: res.totalPages,
          pageSizeOption: this.page.pageSizeOption,
        };
      }),
    );
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
