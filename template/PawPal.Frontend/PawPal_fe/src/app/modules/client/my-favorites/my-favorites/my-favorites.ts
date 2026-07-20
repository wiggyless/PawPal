import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ListAnimal, GetPostQuery } from '../../../../api-services/animal-posts/animal-posts.model';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { PageResult } from '../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { GetMainImagePostBlobClass } from '../../../../api-services/animal-post-images/animal-post-images-model';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { DomSanitizer } from '@angular/platform-browser';
@Component({
  selector: 'app-my-favorites',
  standalone: false,
  templateUrl: './my-favorites.html',
  styleUrl: './my-favorites.scss',
})
export class MyFavorites
  extends BaseListPagedComponent<ListAnimal, GetPostQuery>
  implements OnInit
{
  animalPostsService = inject(AnimalPostService);
  animalPostList: Observable<PageResult<ListAnimal>> | undefined;
  currentUser = inject(CurrentUserService);
  postImages = inject(PostImagesService);
  envLink = environment;
  router = inject(Router);
  cd = inject(ChangeDetectorRef);
  constructor(crr: CurrentUserService) {
    super();
    this.currentUser = crr;
    this.request = new GetPostQuery();
    this.request.paging.pageSize = 4;
  }
  isLoaded = false;
  protected override loadPagedData(): void {}

  page = {
    pageSize: 10,
    currentPage: 1,
    includedTotal: true,
    totalItems: 5,
    totalPages: 0,
    pageSizeOption: [4, 8],
  };

  catalogImages: GetMainImagePostBlobClass[] = [];
  tempList: number[] = [];
  imagesLoaded = signal(false);
  sanitizer = inject(DomSanitizer);
  private sub: Subscription = new Subscription();
  ngOnInit(): void {
    this.loadAnimalPosts();
  }
  ngOnDestroy() {
    this.sub.unsubscribe();
  }
  loadAnimalPosts(): void {
    this.animalPostList = this.animalPostsService.listAnimalPosts({
      userID: this.currentUser.userId() as number,
      isLiked: true,
      paging: this.request.paging,
    });
    this.sub = this.animalPostList.subscribe((res) => {
      this.imagesLoaded.set(true);
    });
  }
  routeToPost(post: ListAnimal) {
    this.router.navigate(['post'], {
      queryParams: {
        postID: post.postID,
        animalID: post.animalID,
        cityID: post.cityID,
        userID: post.userID,
        dateAdded: post.dateAdded,
      },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadAnimalPosts();
  }
  getPostImage(imagePath: string) {
    return this.sanitizer.bypassSecurityTrustUrl(this.envLink.apiUrl + imagePath);
  }
}
