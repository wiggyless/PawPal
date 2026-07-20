import { Component, inject, OnInit, ChangeDetectorRef, signal } from '@angular/core';
import { AnimalCategoriesService } from '../../../../api-services/animal-categories/animal-categories.service';
import { AnimalBreedService } from '../../../../api-services/animal-breed/animal-breed.service';
import { ListAnimalBreedQueryDto } from '../../../../api-services/animal-breed/animal-breed.model';
import { ListAnimalCategoriesQueryDto } from '../../../../api-services/animal-categories/animal-categories.model';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { GetPostQuery, ListAnimal } from '../../../../api-services/animal-posts/animal-posts.model';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { GenderService } from '../../../../api-services/gender/gender-service';
import { GenderEnum, ListGenderDto } from '../../../../api-services/gender/gender-model';
import { CantonsService } from '../../../../api-services/cantons/cantons-service';
import { ActivatedRoute, Router } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { forkJoin, Observable } from 'rxjs';
import { shareReplay, tap } from 'rxjs/operators';
import { PageResult } from '../../../../core/models/paging/page-result';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { PageEvent } from '@angular/material/paginator';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { ListCantonsDto } from '../../../../api-services/cantons/cantons-model';
import { LikedPostsService } from '../../../../api-services/likedPosts/likedPosts-service';
import { environment } from '../../../../../environments/environment';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-catalog',
  standalone: false,
  templateUrl: './catalog.html',
  styleUrl: './catalog.scss',
})
export class CatalogComponent
  extends BaseListPagedComponent<ListAnimal, GetPostQuery>
  implements OnInit
{
  currentUser = inject(CurrentUserService);
  animalCatService = inject(AnimalCategoriesService);
  animalBreedService = inject(AnimalBreedService);
  animalPostsService = inject(AnimalPostService);
  genderService = inject(GenderService);
  cantonsService = inject(CantonsService);
  router = inject(Router);
  postImages = inject(PostImagesService);
  cd = inject(ChangeDetectorRef);
  activeRoute = inject(ActivatedRoute);
  likedPosts = inject(LikedPostsService);
  private fb = inject(FormBuilder);
  page = {
    pageSize: 4,
    currentPage: 1,
    includedTotal: true,
    totalItems: 0,
    totalPages: 0,
    pageSizeOption: [4, 16, 32],
  };

  animalCategories: PageResult<ListAnimalCategoriesQueryDto> | undefined;
  animalBreed: PageResult<ListAnimalBreedQueryDto> | undefined;
  animalPosts: Observable<PageResult<ListAnimal>> = new Observable<PageResult<ListAnimal>>();
  genderList: PageResult<ListGenderDto> | undefined;
  cantonsList: PageResult<ListCantonsDto> | undefined;
  breedArr: Array<ListAnimalBreedQueryDto> = new Array<ListAnimalBreedQueryDto>();
  postArr: Observable<PageResult<ListAnimal>> = new Observable<PageResult<ListAnimal>>();

  filterForm = new FormGroup({
    selectedCat: new FormControl<string>(''),
    selectedBreed: new FormControl<string>(''),
    selectedGender: new FormControl<string>(''),
    selectedCanton: new FormControl<ListCantonsDto>({
      id: null,
      fullName: '',
      cities: [],
    }),
    selectedCity: new FormControl<string>(''),
  });
  ageValue: string = '';
  datePicker = new FormGroup({
    start: new FormControl<any>(null),
    end: new FormControl<any>(null),
  });
  favoritePostList: number[] = [];
  deleteFavoritePostList: number[] = [];
  env = environment;
  tempList: number[] = [];
  cantonID: number | undefined;
  imagesLoaded = signal(true);
  sanitizer = inject(DomSanitizer);
  genderEnum = GenderEnum;
  constructor() {
    super();
    this.request = new GetPostQuery();
    this.request.paging.pageSize = 4;
    this.request.paging.page = 1;
    const navigation = this.router.currentNavigation();
    this.cantonID = navigation?.extras.state?.['cantonID'];
  }

  ngOnInit(): void {
    this.loadPagedData();
  }
  getValue(valueName: string) {
    return this.filterForm.get(valueName)?.value;
  }
  protected override loadPagedData(): void {
    forkJoin({
      categories: this.animalCatService.listAnimalCategories(),
      cantons: this.cantonsService.listCantons(),
      gender: this.genderService.listGender(),
    }).subscribe({
      next: (response) => {
        this.animalCategories = response.categories;
        this.cantonsList = response.cantons;
        this.genderList = response.gender;
        const state = history.state;
        const categoryName = this.activeRoute.snapshot.queryParamMap.get('categoryName');
        if (state != null && categoryName != null) {
          this.filterForm.setValue({
            selectedBreed: '',
            selectedGender: '',
            selectedCity: '',
            selectedCanton: this.cantonsList.items.find((x) => x.id == state.cantonID) ?? {
              id: null,
              fullName: '',
              cities: [],
            },
            selectedCat: categoryName,
          });
          this.searchCatalog();
        } else this.loadPosts();
      },
    });
  }
  loadPosts() {
    this.animalPosts = this.animalPostsService.listAnimalPosts(this.request).pipe(
      shareReplay(1),
      tap((res) => {
        this.likedPosts
          .listLikedPosts({
            userId: this.currentUser.userId() as number,
            postIdList: res.items.map((x) => x.postID),
          })
          .subscribe((response) => {
            this.favoritePostList = response.postList!;
            this.imagesLoaded.set(true);
            this.cd.detectChanges();
          });
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
    this.cd.detectChanges();
  }
  getImageUrl(imagePath: string): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(this.env.apiUrl + imagePath);
  }
  getBreedSelect(): void {
    this.animalBreedService
      .listAnimalBreed({ searchName: '', searchCategoryName: this.getValue('selectedCat') })
      .subscribe((response) => {
        this.breedArr = response.items;
        this.cd.detectChanges();
      });
  }
  compareDates(postDate: Date) {
    var postTime = new Date(postDate);
    var chosenTimeMin =
      this.datePicker.value.start?.getTime() == null
        ? new Date(1970, 1, 1).getTime()
        : this.datePicker.value.start?.getTime();
    var chosenTimeMax =
      this.datePicker.value.end?.getTime() == null
        ? new Date().getTime()
        : this.datePicker.value.end?.getTime();
    return postTime.getTime() >= chosenTimeMin! && postTime.getTime() <= chosenTimeMax!;
  }
  searchCatalog(): void {
    this.mapRequest();
    this.loadPosts();
  }
  clearSearch(): void {
    this.filterForm.reset();
    this.breedArr = new Array<ListAnimalBreedQueryDto>();
    this.ageValue = '';
    this.datePicker.patchValue({
      start: null,
      end: null,
    });
    this.mapRequest();
    this.loadPosts();
  }
  changeCity() {
    this.filterForm.patchValue({
      selectedCity: null,
    });
    this.cd.detectChanges();
  }
  routeToPost(post: ListAnimal) {
    this.router.navigate(['post'], {
      queryParams: {
        postID: post.postID,
      },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadPosts();
  }

  isFavorite(index: number): boolean {
    return this.favoritePostList.find((x) => x == index) == undefined ? false : true;
  }
  likeUnlikePost(index: number, event: Event) {
    if (this.currentUser.getDefaultRoute() == '/login') {
      this.router.navigate(['/auth/login']);
    } else {
      event.stopPropagation();
      if (this.favoritePostList.find((x) => x == index) != undefined) {
        let indexNum = this.favoritePostList.findIndex((x) => x == index);
        this.favoritePostList.splice(indexNum, 1);
        this.deleteFavoritePostList.push(index);
        this.likedPosts
          .deletePost({ userId: this.currentUser.userId() as number, postId: index })
          .subscribe();
      } else {
        this.favoritePostList.push(index);
        this.likedPosts
          .addLikedPosts({ userID: this.currentUser.userId() as number, postID: index })
          .subscribe();
      }
    }
  }
  mapRequest() {
    this.request = {
      searchCategoryName: this.getValue('selectedCat'),
      searchBreed: this.getValue('selectedBreed'),
      searchCityName: this.getValue('selectedCity'),
      searchDateAddedMax: this.datePicker.value.end,
      searchDateAddedMin: this.datePicker.value.start,
      searchGender: this.getValue('selectedGender'),
      searchCantonId: this.getValue('selectedCanton')?.id,
      paging: this.request.paging,
    };
  }
}
