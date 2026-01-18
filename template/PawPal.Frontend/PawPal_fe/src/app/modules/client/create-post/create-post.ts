import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { GenderService } from '../../../api-services/gender/gender-service';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { AnimalBreedService } from '../../../api-services/anima-breed/animal-breed.service';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalUserService } from '../../../api-services/animal-users/animal-users-service';
import { CitiesService } from '../../../api-services/cities/cities.service';
import { GetUserByIdDto } from '../../../api-services/animal-users/animal-users-model';
import { Location } from '@angular/common';
import { PostImagesService } from '../../../api-services/animal-post-images/animal-post-images-service';
import {
  AddNewPostImages,
  GetImagePostBlob,
  GetPostImageById,
} from '../../../api-services/animal-post-images/animal-post-images-model';

import { forkJoin, map, Observable, switchMap } from 'rxjs';
import { ActivatedRoute, Route, Router } from '@angular/router';
import {
  AddAnimalPost,
  AnimalPostByIdQuery,
} from '../../../api-services/animal-posts/animal-posts.model';
import { AddAnimalDto, UpdateAnimalDto } from '../../../api-services/animals/animal-model';
import { AnimalService } from '../../../api-services/animals/animal';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { ListAnimalBreedQueryDto } from '../../../api-services/anima-breed/animal-breed.model';
import { AllergyService } from '../../../api-services/alergies/allergy-service';
import { DisabilityService } from '../../../api-services/disabilities/disability-service';
import { AnimalCategoryByIdQueryDto } from '../../../api-services/animal-categories/animal-categories.model';
import {
  AddAnimalHealthHistory,
  UpdateHealthHistory,
} from '../../../api-services/animals-health/animals-health-model';
import { AnimalsHealthService } from '../../../api-services/animals-health/animals-health-service';
import { AllergyQueryDto } from '../../../api-services/alergies/allergy-model';
import { DisabilitiesDto } from '../../../api-services/disabilities/disability-model';
@Component({
  selector: 'app-create-post',
  standalone: false,
  templateUrl: './create-post.html',
  styleUrl: './create-post.scss',
  providers: [provideNativeDateAdapter()],
})
export class CreatePost implements OnInit {
  isPassChecked = false;
  // injections
  private _formBuilder = inject(FormBuilder);
  genderService = inject(GenderService);
  categoryService = inject(AnimalCategoriesService);
  breedService = inject(AnimalBreedService);
  currentUser = inject(CurrentUserService);
  animalUserService = inject(AnimalUserService);
  cityService = inject(CitiesService);
  animalService = inject(AnimalService);
  postService = inject(AnimalPostService);
  route = inject(ActivatedRoute);
  location = inject(Location);
  postImages = inject(PostImagesService);
  nextRoute = inject(Router);
  cd = inject(ChangeDetectorRef);
  //Group forms
  firstFormGroup = this._formBuilder.group({
    firstCtrl: this._formBuilder.array<string>([]),
  });
  get imagesL() {
    return this.firstFormGroup.get('firstCtrl') as FormArray;
  }
  get imageControls() {
    return (this.firstFormGroup.get('firstCtrl') as FormArray).controls as FormControl[];
  }
  secondFormGroup = this._formBuilder.group({
    name: ['', Validators.required],
    genderID: [0, Validators.required],
    categoryID: [0, Validators.required],
    age: [0, Validators.required],
    breed: ['', Validators.required],
    passportCtrl: [{ value: '', disabled: true }, Validators.required],
    passportCheck: [false],
  });
  thridFormGroup = this._formBuilder.group({
    lastDateCtrl: [{ value: new Date(), disabled: false }],
    lastDateCheck: [false, Validators.required],
    allergyCtrl: [{ value: '', disabled: true }],
    allergyCheck: [false],
    disCtrl: [{ value: '', disabled: true }],
    disCheck: [false],
    dateCtrl: [{ value: '' }],
    vaccineCheck: [false],
    sterCheck: [false],
    parasiteCheck: [false],
  });
  fourthFromGroup = this._formBuilder.group({
    images: this.firstFormGroup,
    mainInfo: this.secondFormGroup,
    healthInfo: this.thridFormGroup,
  });

  // injections

  allergyService = inject(AllergyService);
  disabilityService = inject(DisabilityService);
  healthHistory = inject(AnimalsHealthService);
  // lists
  genderList: any = [];
  categoryList: any = [];
  breedList: any = [];
  allergyList: any = [];
  disabilityList: any = [];
  // random variables
  env = environment.apiUrl;
  url: string = '';
  substr: string = '';
  picker: any;
  routePostID: number = 0;
  isUpdate: boolean = false;
  routeAnimalID: number = 0;
  /* iris additions for improving logic!!!!!*/
  breedArr: Array<ListAnimalBreedQueryDto> = new Array<ListAnimalBreedQueryDto>();
  selectedCategoryId: any;
  selectedCategory: AnimalCategoryByIdQueryDto = {
    id: 0,
    categoryName: '',
    isEnabled: true,
  };
  selectedGender: any;
  selectedParasiteFree: boolean = false;
  selectedVaccinated: boolean = false;
  selectedSprayed: boolean = false;
  selectedAllergies: any = [];
  selectedDisabilities: any = [];
  //imam id kategorije...
  userData: GetUserByIdDto = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    dateTime: '',
    city: '',
    cantonAbbrevation: '',
    cityID: 0,
  };
  isDateRequired: boolean = true;
  isDiseaseChecked: boolean = false;
  isAllergyChecked: boolean = false;
  currentDate: string = new Date().toLocaleDateString();
  newPost: AddAnimalPost = {
    animalID: 0,
    cityID: 0,
    userId: 0,
    status: false,
  };
  newAnimal: AddAnimalDto = {
    name: '',
    breed: '',
    genderId: 0,
    categoryId: 0,
    age: 0,
    hasPapers: false,
    childFriendly: false,
  };
  newPostIamge: AddNewPostImages = {
    postId: 0,
    postImages: [],
  };
  updateHealth: UpdateHealthHistory = {
    animalId: 0,
    vaccinated: false,
    spayedOrNeutered: false,
    parasiteFree: false,
    dietaryRestrictions: '',
    allergies: [],
    disabilities: [],
  };
  updateAnimal: UpdateAnimalDto = {
    name: '',
    breed: '',
    gender: '',
    age: 0,
    hasPapers: true,
    childFriendly: true,
    category: '',
    categoryID: 0,
  };
  index: number = 0;
  imgFileList: Array<File> = Array();
  imageObserveList: Observable<GetImagePostBlob> | undefined;
  tempList: string[] = [];
  tempListFile: File[] = [];
  newAnimalHealthHistory: AddAnimalHealthHistory = {
    animalId: 0,
    parasiteFree: false,
    spayedOrNeutered: false,
    vaccinated: false,
    animalDisabilities: [],
    animalAllergies: [],
    dietaryRestrictions: '',
  };
  newAnimalId: number = 0;
  secondStep: boolean = false;
  newImage = true;
  //--Functions--//
  ngOnInit(): void {
    const params = this.route.snapshot.queryParams;
    if (Object.keys(params).length != 0) {
      this.route.queryParams.subscribe((params) => {
        this.routePostID = params['postID'];
        this.isUpdate = params['update'];
        this.routeAnimalID = params['animalID'];
      });
    }
    this.loadServices();
  }
  changeStep() {
    this.secondStep = !this.secondStep;
  }
  loadServices() {
    if (this.isUpdate) {
      this.loadUpdatePage();
      //this.loadImages();
    } else {
      forkJoin({
        genders: this.genderService.listGender(),
        categories: this.categoryService.listAnimalCategories(),
        breeds: this.breedService.listAnimalBreed(),
        user: this.animalUserService.getUser(this.currentUser.userId),
        allergies: this.allergyService.listAnimalAllergies(),
        disabilities: this.disabilityService.listAnimalDisability(),
      }).subscribe({
        next: (results) => {
          this.genderList = results.genders;
          this.categoryList = results.categories;
          this.breedList = results.breeds;
          this.userData = results.user;
          this.allergyList = results.allergies;
          this.disabilityList = results.disabilities;
          this.cd.detectChanges();
          // Now you are 100% sure ALL data is ready for the form
        },
        error: (err) => {
          console.error('One of the requests failed', err);
        },
      });
      /*
    this.genderService.listGender().subscribe((response) => {
      this.genderList = response;
    });
    this.categoryService.listAnimalCategories().subscribe((response) => {
      this.categoryList = response;
    });
    this.breedService.listAnimalBreed().subscribe((response) => {
      this.breedList = response;
    });
    this.animalUserService.getUser(this.currentUser.userId).subscribe((response) => {
      this.userData = response;
    });
    this.allergyService.listAnimalAllergies().subscribe((res) => {
      this.allergyList = res;
    });
    this.disabilityService.listAnimalDisability().subscribe((res) => {
      this.disabilityList = res;
    });
    */
    }
  }
  loadUpdatePage() {
    forkJoin({
      posts: this.postService.getPostById(this.routePostID),
      health: this.healthHistory.getAnimalHealthHistoryById(this.routeAnimalID),
      genders: this.genderService.listGender(),
      categories: this.categoryService.listAnimalCategories(),
      breeds: this.breedService.listAnimalBreed(),
      user: this.animalUserService.getUser(this.currentUser.userId),
      allergies: this.allergyService.listAnimalAllergies(),
      disabilities: this.disabilityService.listAnimalDisability(),
      images: this.postImages.getImagePostBlob(this.routePostID),
    }).subscribe({
      next: (results) => {
        this.genderList = results.genders;
        this.categoryList = results.categories;
        this.breedList = results.breeds;
        this.userData = results.user;
        this.allergyList = results.allergies;
        this.disabilityList = results.disabilities;
        this.secondFormGroup.patchValue(results.posts);
        this.thridFormGroup.patchValue({
          allergyCtrl: results.health.animalAllergies.toString(),
          allergyCheck: results.health.animalAllergies.toString() != '' ? true : false,
          disCtrl: results.health.animalDisabilities.toString(),
          disCheck: results.health.animalDisabilities.toString() != '' ? true : false,
          vaccineCheck: results.health.vaccinated,
          parasiteCheck: results.health.parasiteFree,
          sterCheck: results.health.spayedOrNeutered,
        });
        this.getBreedSelect();
        this.loadBlob(results.images);
        // Now you are 100% sure ALL data is ready for the form
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('One of the requests failed', err);
      },
    });
  }
  showImages(event: any): void {
    this.newImage = false;
    this.index = this.imageControls.length;
    if (this.index == 10) return;
    for (let i = 0; i < event.target.files.length; i++) {
      const files = event.target.files[i];
      const reader = new FileReader();
      this.imgFileList.push(files);
      reader.readAsDataURL(files);
      reader.onload = () => {
        this.imageControls.push(this._formBuilder.control(reader.result?.toString() as string));
        this.cd.detectChanges();
        this.firstFormGroup.updateValueAndValidity();
        this.newImage = true;
      };
    }
  }
  loadImages(): void {
    this.imageObserveList = this.postImages.getImagePostBlob(this.routePostID);
    this.imageObserveList.subscribe((response) => {
      this.loadBlob(response);
    });
  }
  loadBlob(items: GetImagePostBlob) {
    this.newImage = false;
    items.postImages.forEach((base64String: string) => {
      const byteCharacters = atob(base64String);
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
      this.imageControls.push(this._formBuilder.control(imageUrl));
    });
    this.firstFormGroup.updateValueAndValidity();
    this.newImage = true;
    this.cd.detectChanges();
    this.createFiles(items);
  }
  createFiles(items: GetImagePostBlob) {
    items.postImages.forEach((base64String: string, index: number) => {
      const base64Data = base64String.includes(',') ? base64String.split(',')[1] : base64String;
      const byteCharacters = atob(base64Data);
      const byteArray = Uint8Array.from(byteCharacters, (char) => char.charCodeAt(0));
      const fileName = `image_${index}_${Date.now()}.png`;
      const file = new File([byteArray], fileName, { type: 'image/png' });
      this.imgFileList.push(file);
      // 5. Create the preview URL (optional, for UI display)
      const imageUrl = URL.createObjectURL(file);
    });
  }
  createImges(): void {
    this.newPostIamge.postId = this.routePostID;
    this.newPostIamge.postImages = this.imgFileList;
    let temp;
    this.postImages.updatePostImages(this.newPostIamge).subscribe((resposne) => {
      temp = resposne;
    });
  }
  deleteImage(index: number): void {
    if (!this.isUpdate) this.imgFileList.splice(index, 1);
    this.imageControls.splice(index, 1);
    this.firstFormGroup.updateValueAndValidity();

    this.cd.detectChanges();
  }
  togglePass(): void {
    !this.secondFormGroup.value.passportCheck
      ? this.secondFormGroup.get('passportCtrl')?.disable()
      : this.secondFormGroup.get('passportCtrl')?.enable();
  }
  toggleDate(): void {
    this.isDateRequired = !this.isDateRequired;
    !this.isDateRequired
      ? this.thridFormGroup.get('dateCtrl')?.disable()
      : this.thridFormGroup.get('dateCtrl')?.enable();
  }
  toggleDis(): void {
    this.isDiseaseChecked = !this.isDiseaseChecked;
    !this.isDiseaseChecked
      ? this.thridFormGroup.get('disCtrl')?.disable()
      : this.thridFormGroup.get('disCtrl')?.enable();
  }
  toggleAllergy(): void {
    this.isAllergyChecked = !this.isAllergyChecked;
    !this.isAllergyChecked
      ? this.thridFormGroup.get('allergyCtrl')?.disable()
      : this.thridFormGroup.get('allergyCtrl')?.enable();
  }
  showImagesCtrl() {
    this.cd.detectChanges();
  }
  updatePost(): void {
    let healthHistoryID = 0;
    console.log(this.routeAnimalID);
    this.healthHistory.getAnimalHealthHistoryById(this.routeAnimalID).subscribe((response) => {
      healthHistoryID = response.animalHealthHistoryId;
      this.selectedAllergies.forEach((element: string) => {
        this.updateHealth?.allergies.push(new AllergyQueryDto(element));
      });
      this.selectedDisabilities.forEach((element: string) => {
        this.updateHealth?.disabilities.push(new DisabilitiesDto(element));
      });
      this.updateHealth.parasiteFree = this.selectedParasiteFree;
      this.updateHealth.vaccinated = this.selectedVaccinated;
      this.updateHealth.spayedOrNeutered = this.selectedSprayed;
      this.updateHealth.animalId = this.routeAnimalID;
      this.updateAnimal = this.fourthFromGroup.value.mainInfo as UpdateAnimalDto;
      this.updateAnimal.gender = 'Female'; // need to change
      this.updateAnimal.category = this.selectedCategory.categoryName;
      this.newPostIamge.postImages = this.imgFileList;
      this.newPostIamge.postId = this.routePostID;
      forkJoin({
        animals: this.animalService.updateAnimal(this.updateAnimal, this.routeAnimalID),
        health: this.healthHistory.updateAnimalHealthHistory(this.updateHealth, healthHistoryID),
        postImages: this.postImages.updatePostImages(this.newPostIamge),
      }).subscribe({
        next: (response) => {
          this.nextRoute.navigate(['']);
        },
      });
    });
  }
  addPost(): void {
    ((this.newAnimal.name = this.fourthFromGroup.value.mainInfo?.name as string),
      (this.newAnimal.age = this.fourthFromGroup.value.mainInfo?.age as number));
    this.newAnimal.breed = this.fourthFromGroup.value.mainInfo?.breed as string;
    this.newAnimal.categoryId = this.fourthFromGroup.value.mainInfo?.categoryID as any;
    this.newAnimal.hasPapers = this.fourthFromGroup.value.mainInfo?.passportCheck as boolean;
    this.newAnimal.name = this.fourthFromGroup.value.mainInfo?.name as string;
    this.newAnimal.genderId = this.fourthFromGroup.value.mainInfo?.genderID as any;

    this.newAnimalHealthHistory.dietaryRestrictions = '';
    this.newAnimalHealthHistory.parasiteFree = this.selectedParasiteFree;
    this.newAnimalHealthHistory.vaccinated = this.selectedVaccinated;
    this.newAnimalHealthHistory.spayedOrNeutered = this.selectedSprayed;
    this.newAnimalHealthHistory.animalAllergies = this.selectedAllergies;
    this.newAnimalHealthHistory.animalDisabilities = this.selectedDisabilities;
    this.newPost.cityID = this.userData.cityID;
    this.newPost.status = true;
    this.newPost.userId = this.userData.id;
    this.newPostIamge.postImages = this.imgFileList;
    let newPostId = 0;
    this.animalService
      .addAnimal(this.newAnimal)
      .pipe(
        switchMap((animalId) => {
          // 1. Prepare and save Health History
          this.newAnimalHealthHistory.animalId = animalId;
          this.newPost.animalID = animalId;
          return this.healthHistory.addAnimalHealthHistory(this.newAnimalHealthHistory);
        }),
        switchMap((healthHistoryResponse) => {
          // 2. Save the Post (Now you have the Health History ID if needed)
          // Assuming healthHistoryResponse contains the ID you mentioned
          return this.postService.addPost(this.newPost);
        }),
        switchMap((postResponse) => {
          // 3. Create the Post Images
          this.newPostIamge.postId = postResponse.id;
          console.log(postResponse);
          return this.postImages.createPostImages(this.newPostIamge);
        }),
      )
      .subscribe({
        next: (finalResult) => {
          this.nextRoute.navigate(['']);
          console.log('All steps completed successfully');
          this;
        },
        error: (err) => {
          console.error('Something went wrong in the chain:', err);
        },
      });
    /*
    this.animalService.addAnimal(this.newAnimal).subscribe((response) => {
      this.newAnimalId = response;

      this.newAnimalHealthHistory.animalId = this.newAnimalId;

      this.healthHistory
        .addAnimalHealthHistory(this.newAnimalHealthHistory)
        .subscribe((response) => {
          console.log(response);
        });


      this.postService.addPost(this.newPost).subscribe((response) => {
        newPostId = response;
      });

    });
    */
  }
  getPageBack() {
    this.location.back();
  }
  getBreedSelect(): void {
    this.selectedCategoryId = this.secondFormGroup.get('categoryID')?.value; //ovo uzima iz mat-selecta
    this.breedArr = this.breedList.items; //gives it all the breeds that are in selectedCategoryId db
    this.breedArr = this.breedArr.filter((x) => x.categoryId == this.selectedCategoryId); //filters it :D
    this.categoryService.getAnimalCategoryById(this.selectedCategoryId).subscribe((res) => {
      this.selectedCategory = res;
    });
  }
  toggleVacc() {
    this.selectedVaccinated = !this.selectedVaccinated;
  }
  toggleSpray() {
    this.selectedSprayed = !this.selectedSprayed;
  }
  toggleParasite() {
    this.selectedParasiteFree = !this.selectedParasiteFree;
  }

  addAllergyToList() {
    this.selectedAllergies = this.thridFormGroup.get('allergyCtrl')?.value;
  }

  addDisabilityToList() {
    this.selectedDisabilities = this.thridFormGroup.get('disCtrl')?.value;
  }
  setGender() {
    this.selectedGender = this.secondFormGroup.get('genderID')?.value;
  }
}
