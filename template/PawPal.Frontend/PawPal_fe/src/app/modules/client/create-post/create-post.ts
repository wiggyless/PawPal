import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { GenderService } from '../../../api-services/gender/gender-service';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { AnimalBreedService } from '../../../api-services/animal-breed/animal-breed.service';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { UserService } from '../../../api-services/users/users-service';
import { CitiesService } from '../../../api-services/cities/cities.service';
import { GetUserByIdDto } from '../../../api-services/users/users-model';
import { Location } from '@angular/common';
import { PostImagesService } from '../../../api-services/animal-post-images/animal-post-images-service';
import { GetImagePostBlob } from '../../../api-services/animal-post-images/animal-post-images-model';

import { forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { ListAnimalBreedQueryDto } from '../../../api-services/animal-breed/animal-breed.model';
import { AllergyService } from '../../../api-services/allergies/allergy-service';
import { DisabilityService } from '../../../api-services/disabilities/disability-service';
import { ListAnimalCategoriesQueryDto } from '../../../api-services/animal-categories/animal-categories.model';
import { AnimalsHealthService } from '../../../api-services/animals-health/animals-health-service';
import { ListAllergyQueryDto } from '../../../api-services/allergies/allergy-model';
import { ListDisabilitiesQueryDto } from '../../../api-services/disabilities/disability-model';
import { base64ToBlobUrl } from '../../shared/utils/image-utils';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import imageCompression from 'browser-image-compression';
import { PageResult } from '../../../core/models/paging/page-result';
import { GenderEnum, ListGenderDto } from '../../../api-services/gender/gender-model';
import { UserImageService } from '../../../api-services/userImage/userImage-service';
import { SafeUrl } from '@angular/platform-browser';
import { DialoguePopupService } from '../../../api-services/dialogue-popup/dialogue-popup.service';

@Component({
  selector: 'app-create-post',
  standalone: false,
  templateUrl: './create-post.html',
  styleUrl: './create-post.scss',
  providers: [provideNativeDateAdapter()],
})
export class CreatePost implements OnInit {
  // --- Injected dependencies ---
  private _formBuilder = inject(FormBuilder);
  genderService = inject(GenderService);
  categoryService = inject(AnimalCategoriesService);
  breedService = inject(AnimalBreedService);
  currentUser = inject(CurrentUserService);
  animalUserService = inject(UserService);
  cityService = inject(CitiesService);
  postService = inject(AnimalPostService);
  route = inject(ActivatedRoute);
  location = inject(Location);
  postImages = inject(PostImagesService);
  nextRoute = inject(Router);
  cd = inject(ChangeDetectorRef);
  dialog = inject(DialoguePopupService);
  allergyService = inject(AllergyService);
  disabilityService = inject(DisabilityService);
  healthHistory = inject(AnimalsHealthService);
  userImageService = inject(UserImageService);
  // --- Reactive form groups ---
  firstFormGroup = this._formBuilder.group({
    firstCtrl: this._formBuilder.array<string>([]),
  });
  secondFormGroup = this._formBuilder.group({
    name: ['', Validators.required],
    genderID: [0, Validators.min(1)],
    categoryID: [0, Validators.min(1)],
    age: [0, [Validators.required, Validators.min(0)]],
    breed: ['', Validators.required],
    passportCtrl: [{ value: '', disabled: true }, Validators.required],
    passportCheck: [false],
  });
  thridFormGroup = this._formBuilder.group({
    lastDateCtrl: [{ value: new Date(), disabled: false }],
    lastDateCheck: [false, Validators.required],
    allergyCtrl: this._formBuilder.control(['']),
    allergyCheck: [false],
    disCtrl: this._formBuilder.control(['']),
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
  isFinalizing = signal(false);

  get imagesL() {
    return this.firstFormGroup.get('firstCtrl') as FormArray;
  }
  get imageControls() {
    return (this.firstFormGroup.get('firstCtrl') as FormArray).controls as FormControl[];
  }

  // --- Lookup data populated from the API ---
  genderList: PageResult<ListGenderDto> | undefined;
  categoryList: PageResult<ListAnimalCategoriesQueryDto> | undefined;
  breedList: PageResult<ListAnimalBreedQueryDto> | undefined;
  allergyList: PageResult<ListAllergyQueryDto> | undefined;
  disabilityList: PageResult<ListDisabilitiesQueryDto> | undefined;
  breedArr: Array<ListAnimalBreedQueryDto> = [];

  // --- Route state ---
  routePostID: number = 0;
  isUpdate: boolean = false;
  routeAnimalID: number = 0;

  // --- Selection state ---
  selectedCategoryId: number = 0;
  selectedCategory: ListAnimalCategoriesQueryDto = {
    id: 0,
    categoryName: '',
    isEnabled: true,
  };
  selectedGender: any;
  selectedMainImageIndex: number | null = null;
  selectedMainImage: string = '';

  // --- Misc UI state ---
  currentDate: string = new Date().toLocaleDateString();
  secondStep = signal(false);
  newImage = true;
  isDragging = false;
  isUploadingImages = signal(false);
  uploadProgress = 0;
  env = environment.apiUrl;

  private readonly maxImages = 10;

  userData: GetUserByIdDto = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    dateTime: '',
    city: '',
    cantonAbbrevation: '',
    username: '',
    cityID: 0,
    photoURL: '',
  };
  imgFileList: Array<File> = [];
  hasLoaded = signal(false);
  imageUrl = signal<SafeUrl | null>(null);
  genderEnum = GenderEnum;
  currentImageIndex = 0;
  // --- Lifecycle ---
  ngOnInit(): void {
    const params = this.route.snapshot.queryParams;
    this.secondStep.set(false);
    if (Object.keys(params).length !== 0) {
      this.route.queryParams.subscribe((params) => {
        this.routePostID = params['postID'];
        this.isUpdate = params['update'];
        this.routeAnimalID = params['animalID'];
      });
    }
    this.loadServices();
  }

  changeStep(): void {
    this.secondStep.set(true);
    this.cd.detectChanges();
  }

  // --- Data loading ---
  /** Lookups shared by both the "create" and "update" flows. */
  private commonLookups() {
    return {
      genders: this.genderService.listGender(),
      categories: this.categoryService.listAnimalCategories(),
      breeds: this.breedService.listAnimalBreed(),
      user: this.animalUserService.getUser(this.currentUser.userId() as number),
      allergies: this.allergyService.listAnimalAllergies(),
      disabilities: this.disabilityService.listAnimalDisability(),
    };
  }

  private applyCommonLookups(results: {
    genders: PageResult<ListGenderDto>;
    categories: PageResult<ListAnimalCategoriesQueryDto>;
    breeds: PageResult<ListAnimalBreedQueryDto>;
    user: GetUserByIdDto;
    allergies: PageResult<ListAllergyQueryDto>;
    disabilities: PageResult<ListDisabilitiesQueryDto>;
  }): void {
    this.genderList = results.genders;
    this.categoryList = results.categories;
    this.breedList = results.breeds;
    this.userData = results.user;
    this.allergyList = results.allergies;
    this.disabilityList = results.disabilities;
  }

  loadServices(): void {
    if (this.isUpdate) {
      this.loadUpdatePage();
      return;
    }
    forkJoin({
      ...this.commonLookups(),
    }).subscribe({
      next: (results) => {
        this.applyCommonLookups(results);
        this.hasLoaded.set(true);
        this.cd.detectChanges();
      },
      error: (err) => console.error('One of the requests failed', err),
    });
  }

  loadUpdatePage(): void {
    forkJoin({
      ...this.commonLookups(),
      posts: this.postService.getPostById(this.routePostID),
      health: this.healthHistory.getAnimalHealthHistoryById(this.routeAnimalID),
      images: this.postImages.getImagePostBlob(this.routePostID),
    }).subscribe({
      next: (results) => {
        this.applyCommonLookups(results);
        this.secondFormGroup.patchValue(results.posts);
        this.thridFormGroup.patchValue({
          allergyCtrl: results.health.animalAllergies.map((x) => x.allergyName),
          allergyCheck: !!results.health.animalAllergies.toString(),
          disCtrl: results.health.animalDisabilities.map((x) => x.disabilityName),
          disCheck: !!results.health.animalDisabilities.toString(),
          vaccineCheck: results.health.vaccinated,
          parasiteCheck: results.health.parasiteFree,
          sterCheck: results.health.spayedOrNeutered,
        });
        this.setGender();
        this.getBreedSelect();
        this.loadBlob(results.images);
      },
      error: (err) => console.error('One of the requests failed', err),
    });
  }

  async showImages(event: any): Promise<void> {
    const files = event.target.files;
    const compressionOptions = {
      maxSizeMB: 1,
      maxWidthOrHeight: 1200,
      useWebWorker: true,
    };

    for (let i = 0; i < files.length; i++) {
      if (this.imageControls.length >= this.maxImages) break;

      const file = files[i];
      try {
        const compressedBlob = await imageCompression(file, compressionOptions);
        const compressedFile = new File([compressedBlob], file.name, {
          type: file.type,
          lastModified: Date.now(),
        });
        const dataUrl = await imageCompression.getDataUrlFromFile(compressedFile);

        this.imageControls.push(this._formBuilder.control(dataUrl));
        this.cd.detectChanges();
        this.imgFileList.push(compressedFile);
      } catch (error) {
        console.error('Compression pipeline failed for file:', file.name, error);
      }
    }
  }

  loadBlob(items: GetImagePostBlob): void {
    this.newImage = false;

    items.postImages.forEach((base64: string, index: number) => {
      const imageUrl = base64ToBlobUrl(base64);
      this.imageControls.push(this._formBuilder.control(imageUrl));

      fetch(imageUrl)
        .then((res) => res.blob())
        .then((blob) => {
          this.imgFileList.push(new File([blob], `image_${index}.png`, { type: 'image/png' }));
        });
    });

    this.newImage = true;
    this.hasLoaded.set(true);
    this.cd.detectChanges();
  }

  deleteImage(index: number): void {
    if (index === this.selectedMainImageIndex) {
      this.selectedMainImageIndex = null;
      this.selectedMainImage = '';
    }
    this.imgFileList.splice(index, 1);
    this.imageControls.splice(index, 1);
    this.firstFormGroup.updateValueAndValidity();
  }

  setMainImage(index: number): void {
    if (this.imageControls.at(index)?.value != null) {
      this.selectedMainImageIndex = this.selectedMainImageIndex === index ? null : index;
      this.selectedMainImage = this.imageControls.at(index)!.value;
      this.cd.detectChanges();
    }
  }

  reorderImages(event: CdkDragDrop<any[]>): void {
    moveItemInArray(this.imageControls, event.previousIndex, event.currentIndex);

    if (event.previousIndex === this.selectedMainImageIndex) {
      this.selectedMainImageIndex = event.currentIndex;
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(_event: DragEvent): void {
    this.isDragging = false;
  }

  onFileDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    const files = event.dataTransfer?.files;
    if (files) {
      this.showImages({ target: { files } } as any);
    }
  }

  toggleFormControl(checkControlName: string, targetControlName: string, group: FormGroup): void {
    const isChecked = group.get(checkControlName)?.value;
    const targetCtrl = group.get(targetControlName);
    isChecked ? targetCtrl?.enable() : targetCtrl?.disable();
  }

  showImagesCtrl(): void {
    this.cd.detectChanges();
  }

  getBreedSelect(): void {
    this.selectedCategoryId = this.secondFormGroup.get('categoryID')?.value as number;
    this.breedArr = this.breedList!.items.filter(
      (x: ListAnimalBreedQueryDto) => x.categoryId === this.selectedCategoryId,
    );
    this.selectedCategory = this.categoryList?.items.find(
      (x) => x.id == this.selectedCategoryId,
    ) as ListAnimalCategoriesQueryDto;
  }

  setGender(): void {
    const genderId = this.secondFormGroup.get('genderID')?.value;
    const gender = this.genderList?.items.find((g: any) => g.id === genderId);
    this.selectedGender = gender ? gender.name : '';
  }

  getPageBack(): void {
    this.location.back();
  }

  /** Builds the single multipart request body shared by addPost()/updatePost() — animal, health, allergies/disabilities and images all go to one atomic backend endpoint. */
  private buildAnimalPostFormData(): FormData {
    const mainInfo = this.secondFormGroup.value;
    const healthInfo = this.thridFormGroup.value;
    const formData = new FormData();

    formData.append('name', mainInfo.name as string);
    formData.append('breed', mainInfo.breed as string);
    formData.append('genderId', String(mainInfo.genderID));
    formData.append('age', String(mainInfo.age));
    formData.append('hasPapers', String(!!mainInfo.passportCheck));
    formData.append('childFriendly', 'false');
    formData.append('categoryId', String(mainInfo.categoryID));

    formData.append('vaccinated', String(!!healthInfo.vaccineCheck));
    formData.append('spayedOrNeutered', String(!!healthInfo.sterCheck));
    formData.append('parasiteFree', String(!!healthInfo.parasiteCheck));
    formData.append('dietaryRestrictions', '');
    ((healthInfo.allergyCtrl as string[]) ?? [])
      .filter((allergy) => !!allergy)
      .forEach((allergy) => formData.append('allergies', allergy));
    ((healthInfo.disCtrl as string[]) ?? [])
      .filter((disability) => !!disability)
      .forEach((disability) => formData.append('disabilities', disability));

    this.imgFileList.forEach((file) => formData.append('postImages', file, file.name));

    return formData;
  }

  private handleAnimalPostUploadEvent(
    event: HttpEvent<unknown>,
    onComplete: () => void,
  ): void {
    if (event.type === HttpEventType.UploadProgress && event.total) {
      this.uploadProgress = Math.round((100 * event.loaded) / event.total);
      if (this.uploadProgress >= 100) {
        this.isUploadingImages.set(false);
        this.isFinalizing.set(true);
      }
      this.cd.detectChanges();
    } else if (event.type === HttpEventType.Response) {
      this.uploadProgress = 100;
      this.isFinalizing.set(false);
      this.cd.detectChanges();
      onComplete();
    }
  }

  updatePost(): void {
    if (this.secondFormGroup.invalid) {
      this.secondFormGroup.markAllAsTouched();
      return;
    }

    const formData = this.buildAnimalPostFormData();
    this.isUploadingImages.set(true);
    this.isFinalizing.set(false);
    this.uploadProgress = 0;
    this.cd.detectChanges();

    this.postService.updateAnimalPost(this.routePostID, formData).subscribe({
      next: (event) =>
        this.handleAnimalPostUploadEvent(event, () => {
          this.isUploadingImages.set(false);
          this.isFinalizing.set(false);
          this.cd.detectChanges();
          this.nextRoute.navigate(['']);
        }),
      error: (err) => {
        this.isUploadingImages.set(false);
        this.isFinalizing.set(false);
        console.error('Failed to update post', err);
      },
    });
  }

  addPost(): void {
    if (this.secondFormGroup.invalid) {
      this.secondFormGroup.markAllAsTouched();
      return;
    }

    const formData = this.buildAnimalPostFormData();
    this.isUploadingImages.set(true);
    this.isFinalizing.set(false);
    this.uploadProgress = 0;
    this.cd.detectChanges();

    this.postService.addAnimalPost(formData).subscribe({
      next: (event) =>
        this.handleAnimalPostUploadEvent(event, () => {
          setTimeout(() => {
            this.isUploadingImages.set(false);
            this.isFinalizing.set(false);
            this.cd.detectChanges();
            this.dialog.success(
              'Post Created',
              'Your post has been created successfully. You can now view it on the catalog!',
              'OK',
            );
            this.nextRoute.navigate(['']);
          }, 1500);
        }),
      error: (err) => {
        this.isUploadingImages.set(false);
        this.isFinalizing.set(false);
        console.error('Something went wrong while creating the post:', err);
      },
    });
  }
  nextImage(length: number): void {
    this.currentImageIndex = this.currentImageIndex === length - 1 ? 0 : this.currentImageIndex + 1;
  }
  prevImage(length: number): void {
    this.currentImageIndex = this.currentImageIndex === 0 ? length - 1 : this.currentImageIndex - 1;
  }

  getTransformStyle(): string {
    return `translateX(-${this.currentImageIndex * 100}%)`;
  }
}
