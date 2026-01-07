import { Component, inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AnimalCategoriesService } from '../../../../api-services/animal-categories/animal-categories.service';
import { AnimalBreedService } from '../../../../api-services/anima-breed/animal-breed.service';
import { ListAnimalBreedQueryDto } from '../../../../api-services/anima-breed/animal-breed.model';
import { ListAnimalCategoriesQueryDto } from '../../../../api-services/animal-categories/animal-categories.model';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { ListAnimalPostsDto } from '../../../../api-services/animal-posts/animal-posts.model';
import { MatInput } from '@angular/material/input';
import { FormControl, FormGroup } from '@angular/forms';
import { GenderService } from '../../../../api-services/gender/gender-service';
import { ListGenderDto } from '../../../../api-services/gender/gender-model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { CantonsService } from '../../../../api-services/cantons/cantons-service';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-catalog',
  standalone: false,
  templateUrl: './catalog.html',
  styleUrl: './catalog.scss',
})
export class CatalogComponent implements OnInit {
  currentUser = inject(CurrentUserService);
  animalCatService = inject(AnimalCategoriesService);
  animalBreedService = inject(AnimalBreedService);
  animalPostsService = inject(AnimalPostService);
  citiesService = inject(CitiesService);
  genderService = inject(GenderService);
  cantonsService = inject(CantonsService);
  router = inject(Router);
  animalCategories: any = [];
  animalBreed: any = [];
  animalPosts: any = [];
  genderList: any = [];
  cantonsList: any = [];
  breedArr: Array<ListAnimalBreedQueryDto> = new Array<ListAnimalBreedQueryDto>();
  postArr: Array<ListAnimalPostsDto> = new Array<ListAnimalPostsDto>();
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
  @ViewChild('breedSelect') breedRef!: ElementRef;
  fromInputMax: MatInput = new MatInput();
  ngOnInit(): void {
    this.loadCategories();
    this.loadAnimalBreed();
    this.loadPosts();
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
  loadPosts(): void {
    this.animalPosts = this.animalPostsService.listAnimalPosts().subscribe((response) => {
      this.animalPosts = response;
      this.postArr = this.animalPosts.items;
    });
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
    this.postArr = this.animalPosts.items;
    this.postArr = (this.animalPosts.items as Array<ListAnimalPostsDto>).filter(
      (x) =>
        (this.selectedCat == null ? true : this.selectedCat == x.categoryID) &&
        (this.selectedBreed == null
          ? true
          : !(this.selectedBreed as string).localeCompare(x.breed)) &&
        this.compareDates(x.dateAdded) &&
        (this.selectedGender == null ? true : this.selectedGender == x.genderID) &&
        (this.selectedCity == null ? true : this.selectedCity == x.cityID)
    );
  }
  clearSearch(): void {
    this.postArr = this.animalPosts.items;
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
}
