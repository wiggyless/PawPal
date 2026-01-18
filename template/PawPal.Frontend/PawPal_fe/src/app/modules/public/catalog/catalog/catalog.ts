import { Component, inject, OnInit, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
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
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { map, shareReplay, switchMap, tap } from 'rxjs/operators';
import { PageResult } from '../../../../core/models/paging/page-result';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { PageEvent } from '@angular/material/paginator';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import {
  GetMainImagePostBlob,
  GetMainImagePostBlobClass,
  ListMainImageId,
} from '../../../../api-services/animal-post-images/animal-post-images-model';

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
  postImages = inject(PostImagesService);
  cd = inject(ChangeDetectorRef);
  // Page Values ( didnt use the template cuz whats going on???)
  page = {
    pageSize: 4,
    currentPage: 1,
    includedTotal: true,
    totalItems: 0,
    totalPages: 0,
    pageSizeOption: [4, 16, 32],
  };

  // Lists //
  animalCategories: any = [];
  animalBreed: any = [];
  animalPosts: Observable<PageResult<ListAnimal>> = new Observable<PageResult<ListAnimal>>();
  genderList: any = [];
  cantonsList: any = [];
  breedArr: Array<ListAnimalBreedQueryDto> = new Array<ListAnimalBreedQueryDto>();
  postArr: Observable<PageResult<ListAnimal>> = new Observable<PageResult<ListAnimal>>();

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
  tempList: number[] = [];
  constructor() {
    super();
    this.request = new GetPostQuery();
    this.request.paging.pageSize = 4;
    this.request.paging.page = 1;
  }
  catalogImages: GetMainImagePostBlobClass[] = [];
  ngOnInit(): void {
    this.loadPagedData();
  }
  /*
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
  loadGender(): void {
    this.genderList = this.genderService.listGender().subscribe((resposne) => {
      this.genderList = resposne;
    });
  }
*/
  protected override loadPagedData(): void {
    this.animalPosts = this.animalPostsService.listAnimalPosts(this.request).pipe(
      shareReplay(1),
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
    this.postArr = this.animalPosts;
    forkJoin({
      categories: this.animalCatService.listAnimalCategories(),
      breed: this.animalBreedService.listAnimalBreed(),
      cantons: this.cantonsService.listCantons(),
      gender: this.genderService.listGender(),
      post: this.animalPosts,
      cities: this.citiesService.listCities(),
    }).subscribe({
      next: (response) => {
        this.animalBreed = response.breed;
        this.animalCategories = response.categories;
        this.cantonsList = response.cantons;
        this.genderList = response.gender;
        this.loadPostImages(response.post.items);
        this.cd.detectChanges();
      },
    });

    /*
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
    this.animalPosts.subscribe((x) => {
      this.loadPostImages(x.items);
    });
    */
  }
  loadPostImages(idList: ListAnimal[]): void {
    idList.forEach((element) => {
      this.tempList.push(element.postID);
    });
    this.postImages.getMainImagePostBlob(this.tempList).subscribe((response) => {
      console.log(response);
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
    this.animalPosts = this.postArr.pipe(
      map((post) => {
        return {
          ...post,
          items: post.items.filter((x) => {
            const matchesCat = this.selectedCat == null || this.selectedCat === x.categoryID;
            const matchesBreed =
              this.selectedBreed == null || (this.selectedBreed as string) === x.breed;
            const matchesDate = this.compareDates(x.dateAdded);
            const matchesGender = this.selectedGender == null || this.selectedGender === x.genderID;
            const matchesCity = this.selectedCity == null || this.selectedCity === x.cityID;

            return matchesCat && matchesBreed && matchesDate && matchesGender && matchesCity;
          }),
        };
      }),
      shareReplay(1),
    );
  }
  clearSearch(): void {
    //this.postArr = this.animalPosts.items;
    this.animalPosts = this.postArr;
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
        dateAdded: post.dateAdded,
      },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadPagedData();
  }
  getPostImage(index: number) {
    return this.catalogImages.find((x) => x.postID == index)?.mainImage;
  }
}
