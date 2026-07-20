import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { forkJoin, Subscription } from 'rxjs';
import { PageResult } from '../../../core/models/paging/page-result';
import { ListAnimal } from '../../../api-services/animal-posts/animal-posts.model';
import { GenderService } from '../../../api-services/gender/gender-service';
import { Router } from '@angular/router';
import { CantonsService } from '../../../api-services/cantons/cantons-service';
import { ListCantonsDto } from '../../../api-services/cantons/cantons-model';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from '../../../../environments/environment';
import { ListAnimalCategoriesQueryDto } from '../../../api-services/animal-categories/animal-categories.model';
import { GenderEnum, ListGenderDto } from '../../../api-services/gender/gender-model';
import { NewsService } from '../../../api-services/news/news.service';
import { ListNewsQuery, ListNewsQueryDto } from '../../../api-services/news/news.model';

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
  newsService = inject(NewsService);
  postList: PageResult<ListAnimal> | undefined;
  cantons: PageResult<ListCantonsDto> | undefined;
  animalCategories: PageResult<ListAnimalCategoriesQueryDto> | undefined;
  latestNews: ListNewsQueryDto | undefined;
  selectedCategory: string | undefined;
  selectedCanton: number = 0;
  mySubcribe: Subscription | undefined;
  router = inject(Router);
  genderListForDisplay: PageResult<ListGenderDto> | undefined;
  cd = inject(ChangeDetectorRef);
  sanitizer = inject(DomSanitizer);
  genderEnum = GenderEnum;
  pagingObject = {
    paging: {
      page: 1,
      pageSize: 4,
    },
  };
  env = environment;
  public myBackgroundImage: string = `${this.env.apiUrl}/StaticImages/header-photo.webp`;
  imagesLoaded = signal(false);
  newsLoaded = signal(false);
  ngOnInit(): void {
    this.mySubcribe = forkJoin({
      posts: this.postService.listAnimalPosts(this.pagingObject),
      cantons: this.cantonService.listCantons(),
      categories: this.animalService.listAnimalCategories(),
      gender: this.genderService.listGender(),
    }).subscribe({
      next: (response) => {
        this.postList = response.posts;
        this.cantons = response.cantons;
        this.genderListForDisplay = response.gender;
        this.animalCategories = response.categories;
        this.imagesLoaded.set(true);
      },
    });

    const latestNewsQuery = new ListNewsQuery();
    latestNewsQuery.paging.page = 1;
    latestNewsQuery.paging.pageSize = 1;
    this.newsService.listNews(latestNewsQuery).subscribe({
      next: (response) => {
        this.latestNews = response.items[0];
        this.newsLoaded.set(true);
      },
      error: () => this.newsLoaded.set(true),
    });
  }
  ngOnDestroy(): void {
    this.mySubcribe?.unsubscribe();
  }
  routeToPost(post: ListAnimal) {
    this.router.navigate(['post'], {
      queryParams: {
        postID: post.postID,
      },
    });
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
  getNewsImageUrl(photoUrl?: string) {
    return this.sanitizer.bypassSecurityTrustUrl(this.env.apiUrl + '/' + (photoUrl ?? ''));
  }
  getNewsPreview(content: string, limit: number = 220): string {
    return content.length > limit ? content.slice(0, limit).trimEnd() + '…' : content;
  }
  routeToNews(item: ListNewsQueryDto) {
    this.router.navigate(['news', item.id]);
  }
}
