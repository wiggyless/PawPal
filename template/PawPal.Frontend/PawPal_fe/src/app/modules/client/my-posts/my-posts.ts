import { Component, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import {
  GetPostQuery,
  ListAnimal,
  listAnimalPostsByUserIdDto,
} from '../../../api-services/animal-posts/animal-posts.model';
import { environment } from '../../../../environments/environment';
import { Observable, tap } from 'rxjs';
import { ListAnimalsDto } from '../../../api-services/animals/animal-model';
import { Router } from '@angular/router';
import { PageResult } from '../../../core/models/paging/page-result';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-my-posts',
  standalone: false,
  templateUrl: './my-posts.html',
  styleUrl: './my-posts.scss',
})
export class MyPosts extends BaseListPagedComponent<ListAnimal, GetPostQuery> implements OnInit {
  currentUser: any;
  animalPostsService = inject(AnimalPostService);
  animalPostList: Observable<PageResult<listAnimalPostsByUserIdDto>> | undefined;
  envLink = environment;
  route = inject(Router);
  constructor(crr: CurrentUserService) {
    super();
    this.currentUser = crr;
    this.request = new GetPostQuery();
    this.request.paging.pageSize = 2;
  }
  protected override loadPagedData(): void {}
  // Page Values ( didnt use the template cuz whats going on???)
  page = {
    pageSize: 10,
    currentPage: 1,
    includedTotal: true,
    totalItems: 5,
    totalPages: 0,
    pageSizeOption: [2, 5, 10, 100],
  };

  ngOnInit(): void {
    this.loadAnimalPosts();
  }
  loadAnimalPosts(): void {
    const userObject = {
      userId: this.currentUser?.userId,
      paging: this.request.paging,
    };
    this.animalPostList = this.animalPostsService.listAnimalPostsByUserId(userObject).pipe(
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
  loadPost(post: listAnimalPostsByUserIdDto): void {
    this.route.navigate(['/post'], {
      queryParams: {
        postID: post.postId,
        animalID: post.animalID,
        cityID: post.cityID,
        userID: post.userId,
      },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadAnimalPosts();
  }
}
