import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { forkJoin, Subscription } from 'rxjs';
import { PageResult } from '../../../core/models/paging/page-result';
import { ListAnimal } from '../../../api-services/animal-posts/animal-posts.model';
import { GetMainImagePostBlobClass } from '../../../api-services/animal-post-images/animal-post-images-model';
import { PostImagesService } from '../../../api-services/animal-post-images/animal-post-images-service';
import { GenderService } from '../../../api-services/gender/gender-service';
import { Router } from '@angular/router';
import { CantonsService } from '../../../api-services/cantons/cantons-service';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from '../../../../environments/environment';
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
  sanitizer = inject(DomSanitizer);
  pagingObject = {
    paging: {
      page: 1,
      pageSize: 4,
    },
  };
  catalogImages: GetMainImagePostBlobClass[] = [];
  env = environment;
  imagesLoaded = signal(false);
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
        this.imagesLoaded.set(true);
        this.cd.detectChanges();
      },
    });
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
  getImageForPost(imagePath: string) {
    return this.sanitizer.bypassSecurityTrustUrl(this.env.apiUrl + imagePath);
  }
}
