import { Component, inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AnimalCategoriesService } from '../../../../api-services/animal-categories/animal-categories.service';
import { AnimalBreedService } from '../../../../api-services/anima-breed/animal-breed.service';
import { ListAnimalBreedQueryDto } from '../../../../api-services/anima-breed/animal-breed.model';
import { ListAnimalCategoriesQueryDto } from '../../../../api-services/animal-categories/animal-categories.model';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import {
  AnimalPostByIdQuery,
  GetPostQuery,
  ListAnimal,
} from '../../../../api-services/animal-posts/animal-posts.model';
import { MatInput } from '@angular/material/input';
import { FormControl, FormGroup } from '@angular/forms';
import { GenderService } from '../../../../api-services/gender/gender-service';
import { ListGenderDto } from '../../../../api-services/gender/gender-model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { CantonsService } from '../../../../api-services/cantons/cantons-service';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, shareReplay, switchMap, tap } from 'rxjs/operators';
import { PageResult } from '../../../../core/models/paging/page-result';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { PageEvent } from '@angular/material/paginator';

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
  // Injections //
  currentUser = inject(CurrentUserService);
  animalCatService = inject(AnimalCategoriesService);
  animalBreedService = inject(AnimalBreedService);
  animalPostsService = inject(AnimalPostService);
  citiesService = inject(CitiesService);
  genderService = inject(GenderService);
  cantonsService = inject(CantonsService);
  router = inject(Router);

  // Page Values ( didnt use the template cuz whats going on???)
  page = {
    pageSize: 10,
    currentPage: 1,
    includedTotal: true,
    totalItems: 5,
    totalPages: 0,
    pageSizeOption: [2, 5, 10, 100],
  };

  // Lists //
  animalCategories: any = [];
  animalBreed: any = [];
  animalPosts: Observable<PageResult<ListAnimal>> | undefined;
  genderList: any = [];
  cantonsList: any = [];
  breedArr: Array<ListAnimalBreedQueryDto> = new Array<ListAnimalBreedQueryDto>();
  postArr: Observable<ListAnimal[]> | undefined;

  // List selections //
  selectedCat: any;
  selectedBreed: any;
  selectedGender: any;
  selectedCanton: any;
  selectedCity: any;
  ageValue: string = '';
  datePicker = new FormGroup({
    start: new FormControl<any>(null),
    end: new FormControl<any>(null),
  });

  // random values

  fromInputMax: MatInput = new MatInput();
  imageUrl = 'https://localhost:7260/posts/Krompir2.jpg';

  constructor() {
    super();
    this.request = new GetPostQuery();
    this.request.paging.pageSize = 2;
  }
  ngOnInit(): void {
    this.loadCategories();
    this.loadAnimalBreed();
    this.loadPagedData();
    this.loadGender();
    this.loadCantons();
  }
  loadCategories(): void {
    this.animalCategories = this.animalCatService.listAnimalCategories().subscribe((response) => {
      this.animalCategories = response;
    });
  }
  loadCantons(): void {
    this.cantonsList = this.cantonsService.listCantons().subscribe((response) => {
      this.cantonsList = response;
    });
  }
  loadAnimalBreed(): void {
    this.animalBreed = this.animalBreedService.listAnimalBreed().subscribe((response) => {
      this.animalBreed = response;
    });
  }
  protected override loadPagedData(): void {
    this.animalPosts = this.animalPostsService.listAnimalPosts(this.request).pipe(
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
    console.log(this.page);
  }
  loadGender(): void {
    this.genderList = this.genderService.listGender().subscribe((resposne) => {
      this.genderList = resposne;
    });
  }
  getBreedSelect(): void {
    this.breedArr = this.animalBreed.items;
    this.breedArr = this.breedArr.filter((x) => x.categoryId == this.selectedCat);
    this.selectedBreed = null;
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
    /*
    this.postArr = this.animalPosts?.pipe(
      map((post) =>
        post.filter(
          (x) =>
            (this.selectedCat == null ? true : this.selectedCat == x.categoryID) &&
            (this.selectedBreed == null
              ? true
              : !(this.selectedBreed as string).localeCompare(x.breed)) &&
            this.compareDates(x.dateAdded) &&
            (this.selectedGender == null ? true : this.selectedGender == x.genderID) &&
            (this.selectedCity == null ? true : this.selectedCity == x.cityID)
        )
      )
    );
    */
  }
  clearSearch(): void {
    //this.postArr = this.animalPosts.items;
    this.breedArr = new Array<ListAnimalBreedQueryDto>();
    this.selectedCat = null;
    this.selectedBreed = null;
    this.selectedGender = null;
    this.selectedCanton = null;
    this.selectedCity = null;
    this.ageValue = '';
    this.datePicker.patchValue({
      start: null,
      end: null,
    });
  }
  changeCity() {
    this.selectedCity = null;
  }
  routeToPost(post: ListAnimal) {
    this.router.navigate(['post'], {
      queryParams: {
        postID: post.postID,
        animalID: post.animalID,
        cityID: post.cityID,
        userID: post.userID,
      },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadPagedData();
  }
}
