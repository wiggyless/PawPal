import { Component, inject, OnInit, signal } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { GetPostQuery, ListAnimal } from '../../../api-services/animal-posts/animal-posts.model';
import { environment } from '../../../../environments/environment';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { PageResult } from '../../../core/models/paging/page-result';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { PageEvent } from '@angular/material/paginator';
import { GenderEnum } from '../../../api-services/gender/gender-model';

@Component({
  selector: 'app-my-posts',
  standalone: false,
  templateUrl: './my-posts.html',
  styleUrl: './my-posts.scss',
})
export class MyPosts extends BaseListPagedComponent<ListAnimal, GetPostQuery> implements OnInit {
  currentUser: any;
  animalPostsService = inject(AnimalPostService);
  animalPostList: Observable<PageResult<ListAnimal>> | undefined;
  envLink = environment;
  isLoaded = false;
  gender = GenderEnum;
  route = inject(Router);
  constructor(crr: CurrentUserService) {
    super();
    this.currentUser = crr;
    this.request = new GetPostQuery();
    this.request.paging.pageSize = 4;
  }
  protected override loadPagedData(): void {}
  page = {
    pageSize: 10,
    currentPage: 1,
    includedTotal: true,
    totalItems: 5,
    totalPages: 0,
    pageSizeOption: [4, 8],
  };
  isEmpty = signal(false);
  ngOnInit(): void {
    this.loadAnimalPosts();
  }
  loadAnimalPosts(): void {
    const userObject = {
      userId: this.currentUser?.userId() as number,
      paging: this.request.paging,
    };
    this.animalPostList = this.animalPostsService.listAnimalPostsByUserId(userObject).pipe(
      tap((res) => {
        if (res.items.length == 0) {
          this.isEmpty.set(true);
        }
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
  loadPost(post: ListAnimal): void {
    this.route.navigate(['/post'], {
      queryParams: {
        postID: post.postID,
      },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadAnimalPosts();
  }
}
