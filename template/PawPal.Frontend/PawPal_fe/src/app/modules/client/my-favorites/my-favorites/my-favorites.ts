import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { Observable, Subscription, tap } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import {
  ListAnimal,
  GetPostQuery,
  listAnimalPostsByUserIdDto,
} from '../../../../api-services/animal-posts/animal-posts.model';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { PageResult } from '../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
//import { LikedPostsService } from '../../../../api-services/likedPosts/likedPosts-service';
//import { GetLikedPostListQuery } from '../../../../api-services/likedPosts/likedPosts-model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  GetMainImagePostBlob,
  GetMainImagePostBlobClass,
} from '../../../../api-services/animal-post-images/animal-post-images-model';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
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
  //favoritesService = inject(LikedPostsService);
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
  //likedPostQuery: GetLikedPostListQuery = {
  //}
  //listPostRange: ListPostsByRange|undefined;
  isLoaded = false;
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
  //images Var
  catalogImages: GetMainImagePostBlobClass[] = [];
  tempList: number[] = [];
  imagesLoaded = false;
  private sub: Subscription = new Subscription();
  ngOnInit(): void {
    this.loadAnimalPosts();
  }
  ngOnDestroy() {
    this.sub.unsubscribe();
  }
  loadAnimalPosts(): void {
    this.animalPostList = this.animalPostsService.listAnimalPosts({
      userID: this.currentUser.userId,
      isLiked: true,
    });
    this.animalPostList.subscribe((response) => {
      this.loadPostImages(response.items);
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
  // images Shit
  getPostImage(index: number) {
    return this.catalogImages.find((x) => x.postID == index)?.mainImage;
  }
  loadPostImages(idList: ListAnimal[]): void {
    idList.forEach((element) => {
      this.tempList.push(element.postID);
    });
    this.postImages.getMainImagePostBlob(this.tempList).subscribe((response) => {
      this.setImages(response);
    });
  }
  setImages(blobList: GetMainImagePostBlob[]): void {
    blobList.forEach((x) => {
      if (x.mainImage != '') {
        const byteCharacters = atob(x.mainImage);
        const byteNumbers = new Array(byteCharacters.length);

        for (let i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        let mimeType = 'image/png';
        if (byteArray.length > 4) {
          const header = byteArray.slice(0, 4);
          let headerHex = '';
          for (let i = 0; i < header.length; i++) {
            headerHex += header[i].toString(16).toUpperCase();
          }
          if (headerHex.startsWith('89504E47')) {
            mimeType = 'image/png';
          } else if (headerHex.startsWith('FFD8FF')) {
            mimeType = 'image/jpeg';
          }
        }
        // 2. Create the Blob from the typed array
        const blob = new Blob([byteArray], { type: mimeType });

        // 3. Create the URL and add to form
        const imageUrl = URL.createObjectURL(blob);
        this.catalogImages.push(new GetMainImagePostBlobClass(x.postID, imageUrl));
      }
    });
    this.imagesLoaded = true;
    this.cd.detectChanges();
  }
}
