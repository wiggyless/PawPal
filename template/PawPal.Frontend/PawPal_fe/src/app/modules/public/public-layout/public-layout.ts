import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { CitiesService } from '../../../api-services/cities/cities.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { forkJoin, Subscription } from 'rxjs';
import { PageResult } from '../../../core/models/paging/page-result';
import { ListAnimal } from '../../../api-services/animal-posts/animal-posts.model';
import {
  GetMainImagePostBlob,
  GetMainImagePostBlobClass,
} from '../../../api-services/animal-post-images/animal-post-images-model';
import { PostImagesService } from '../../../api-services/animal-post-images/animal-post-images-service';
import { GenderService } from '../../../api-services/gender/gender-service';
import { Router } from '@angular/router';
import { CantonsService } from '../../../api-services/cantons/cantons-service';
@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.scss',
})
export class PublicLayout implements OnInit, OnDestroy {
  animalService = inject(AnimalCategoriesService);
  cantonService = inject(CantonsService);
  currentUser = inject(CurrentUserService);
  postService = inject(AnimalPostService);
  genderService = inject(GenderService);
  postList: PageResult<ListAnimal> | undefined;
  cantons: any = [];
  animalCategories: any = [];
  selectedCategory: any;
  selectedCanton: number = 0;
  tempList: number[] = [];
  mySubcribe: Subscription | undefined;
  postImages = inject(PostImagesService);
  router = inject(Router);
  genderListForDisplay: any = [];
  cd = inject(ChangeDetectorRef);
  pagingObject = {
    paging: {
      page: 1,
      pageSize: 4,
    },
  };
  catalogImages: GetMainImagePostBlobClass[] = [];
  imagesLoaded: boolean = false;
  ngOnInit(): void {
    this.mapGenderToText();
    this.mySubcribe = forkJoin({
      posts: this.postService.listAnimalPosts(this.pagingObject),
      cantons: this.cantonService.listCantons(),
      categories: this.animalService.listAnimalCategories(),
    }).subscribe({
      next: (response) => {
        this.postList = response.posts;
        this.cantons = response.cantons;
        this.animalCategories = response.categories;
        this.loadPostImages(response.posts.items);
        this.cd.detectChanges();
      },
    });
  }
  loadPostImages(idList: ListAnimal[]): void {
    idList.forEach((element) => {
      this.tempList.push(element.postID);
    });
    // this is for loading likedPosts
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
        const blob = new Blob([byteArray], { type: mimeType });
        const imageUrl = URL.createObjectURL(blob);
        this.catalogImages.push(new GetMainImagePostBlobClass(x.postID, imageUrl));
      }
    });
    this.imagesLoaded = true;
    this.cd.detectChanges();
  }
  ngOnDestroy(): void {
    this.mySubcribe?.unsubscribe();
  }
  mapGenderToText() {
    this.genderService.listGender().subscribe((res) => {
      this.genderListForDisplay = res;
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
  showCategory() {
    console.log(this.selectedCategory);
  }
  searchCatalog() {
    this.router.navigate(['/catalog'], {
      state: {
        cantonID: this.selectedCanton,
      },
      queryParams: {
        categoryName: this.selectedCategory,
      },
    });
  }
  getImageForPost(postID: number) {
    return this.catalogImages.find((x) => x.postID == postID)?.mainImage;
  }
}
