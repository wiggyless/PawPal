import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { GenderService } from '../../../api-services/gender/gender-service';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { AnimalBreedService } from '../../../api-services/anima-breed/animal-breed.service';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalUserService } from '../../../api-services/animal-users/animal-users-service';
import { CitiesService } from '../../../api-services/cities/cities.service';
import { GetUserByIdDto } from '../../../api-services/animal-users/animal-users-model';
import { AddAnimalPost } from '../../../api-services/animal-posts/animal-posts.model';
import { AddAnimalDto } from '../../../api-services/animals/animal-model';
import { AnimalService } from '../../../api-services/animals/animal';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { ListAnimalBreedQueryDto } from '../../../api-services/anima-breed/animal-breed.model';
import { AllergyService } from '../../../api-services/alergies/allergy-service';
import { DisabilityService } from '../../../api-services/disabilities/disability-service';
import { AnimalCategoryByIdQueryDto } from '../../../api-services/animal-categories/animal-categories.model';
import { AddAnimalHealthHistory } from '../../../api-services/animals-health/animals-health-model';
import { AnimalsHealthService } from '../../../api-services/animals-health/animals-health-service';
@Component({
  selector: 'app-create-post',
  standalone: false,
  templateUrl: './create-post.html',
  styleUrl: './create-post.scss',
  providers: [provideNativeDateAdapter()],
})
export class CreatePost implements OnInit {


  private _formBuilder = inject(FormBuilder);
  imageList: string[] = ['', '', '', '', '', '', '', '', '', ''];
  isPassChecked = false;
  //Group forms
  firstFormGroup = this._formBuilder.group({
    firstCtrl: new FormControl<string>(''),
  });
  secondFormGroup = this._formBuilder.group({
    nameCtrl: ['', Validators.required],
    genderCtrl: [null, Validators.required],
    categoryCtrl: [null, Validators.required],
    ageCtrl: [0, Validators.required],
    breedCtrl: ['', Validators.required],
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
    dateCtrl: [{value: ''}],
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
  genderService = inject(GenderService);
  categoryService = inject(AnimalCategoriesService);
  breedService = inject(AnimalBreedService);
  currentUser = inject(CurrentUserService);
  animalUserService = inject(AnimalUserService);
  cityService = inject(CitiesService);
  animalService = inject(AnimalService);
  postService = inject(AnimalPostService);
  allergyService = inject(AllergyService)
  disabilityService = inject(DisabilityService);
  healthHistory = inject(AnimalsHealthService);
  // lists
  genderList: any = [];
  categoryList: any = [];
  breedList: any = [];
  fileteredImageList: string[] = [];
  allergyList: any = [];
  disabilityList: any = [];
  // random variables
  env = environment.apiUrl;
  url: string = '';
  substr: string = '';
  picker: any;
  
  /* iris additions for improving logic!!!!!*/
  breedArr: Array<ListAnimalBreedQueryDto> = new Array<ListAnimalBreedQueryDto>();
  selectedCategoryId : any;
  selectedCategory : AnimalCategoryByIdQueryDto = {
    id: 0,
    categoryName: '',
    isEnabled:true
  };
  selectedGender: string = '';
  selectedParasiteFree : boolean = false;
  selectedVaccinated : boolean = false;
  selectedSprayed : boolean = false;
  selectedAllergies : any = [];
  selectedDisabilities : any = [];

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
  newAnimalHealthHistory: AddAnimalHealthHistory = {
    animalId : 0,
    parasiteFree: false,
    spayedOrNeutered: false,
    vaccinated : false,
    animalDisabilities : [],
    animalAllergies : [],
    dietaryRestrictions : ''
  }

   
 newAnimalId: number = 0;
  //--Functions--//

  ngOnInit(): void {
    this.loadServices();
  }
  loadServices() {
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
    })
    this.disabilityService.listAnimalDisability().subscribe((res)=>{
      console.log(res);
      this.disabilityList = res;
    })
  }
  showImages(event: any): void {
    let index = this.imageList.indexOf('');
    if (index == -1) return;
    for (let i = 0; i < event.target.files.length; i++) {
      const files = event.target.files[i];
      const reader = new FileReader();
      reader.readAsDataURL(files);
      reader.onload = () => {
        this.imageList[index] = reader.result?.toString() as string;
        index = this.imageList.indexOf('');
      };
    }
  }
  togglePass(): void {
    !this.secondFormGroup.value.passportCheck
      ? this.secondFormGroup.get('passportCtrl')?.disable()
      : this.secondFormGroup.get('passportCtrl')?.enable();
  }
  toggleDate(): void {
    this.isDateRequired = !this.isDateRequired;
    !this.isDateRequired ? this.thridFormGroup.get('dateCtrl')?.disable() 
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
  filterImages(): void {
    this.fileteredImageList = this.imageList.filter((img) => img != '');
  }
  addPost(): void {
    this.newAnimal.age = this.fourthFromGroup.value.mainInfo?.ageCtrl as number;
    this.newAnimal.breed = this.fourthFromGroup.value.mainInfo?.breedCtrl as string;
    this.newAnimal.categoryId = this.fourthFromGroup.value.mainInfo?.categoryCtrl as any;
    this.newAnimal.hasPapers = this.fourthFromGroup.value.mainInfo?.passportCheck as boolean;
    this.newAnimal.name = this.fourthFromGroup.value.mainInfo?.nameCtrl as string;
    this.newAnimal.genderId = this.fourthFromGroup.value.mainInfo?.genderCtrl as any;

    this.newAnimalHealthHistory.dietaryRestrictions = '';
    this.newAnimalHealthHistory.parasiteFree = this.selectedParasiteFree;
    this.newAnimalHealthHistory.vaccinated = this.selectedVaccinated;
    this.newAnimalHealthHistory.spayedOrNeutered = this.selectedSprayed;
    this.newAnimalHealthHistory.animalAllergies = this.selectedAllergies;
    this.newAnimalHealthHistory.animalDisabilities = this.selectedDisabilities;


    console.log(this.newAnimal);
    console.log(this.newAnimalHealthHistory);
     let newPostId = 0;
    this.animalService.addAnimal(this.newAnimal).subscribe((response) => {
      this.newAnimalId= response;

      this.newAnimalHealthHistory.animalId = this.newAnimalId;
      
      this.healthHistory.addAnimalHealthHistory(this.newAnimalHealthHistory).subscribe((response)=>{
        console.log(response);
      })
      this.newPost.animalID = this.newAnimalId;
      this.newPost.cityID = this.userData.cityID;
      this.newPost.status = true;
      this.newPost.userId = this.userData.id;

       this.postService.addPost(this.newPost).subscribe((response) => {
        newPostId = response;
      });
    });
  }
  getBreedSelect() : void {
      this.selectedCategoryId = this.secondFormGroup.get('categoryCtrl')?.value; //ovo uzima iz mat-selecta
      this.breedArr = this.breedList.items; //gives it all the breeds that are in selectedCategoryId db
      this.breedArr = this.breedArr.filter((x) => x.categoryId == this.selectedCategoryId) //filters it :D
      this.categoryService.getAnimalCategoryById(this.selectedCategoryId).subscribe((res) =>{
        this.selectedCategory = res;
      })
}
     toggleVacc() {
        this.selectedVaccinated = !this.selectedVaccinated;
    }
    toggleSpray() {
      this.selectedSprayed = !this.selectedSprayed;
    }
    toggleParasite(){
      this.selectedParasiteFree = !this.selectedParasiteFree;
    }
    
    addAllergyToList(){
      this.selectedAllergies = this.thridFormGroup.get('allergyCtrl')?.value;
    }

    addDisabilityToList(){
      this.selectedDisabilities = this.thridFormGroup.get('disCtrl')?.value;
    }
}

/*@if(post.genderID === genderList.items[0].id){
              <p>{{ genderList.items[0].name }}</p>
              }
              @else {
              <p>{{ genderList.items[1].name }}</p>
              } */