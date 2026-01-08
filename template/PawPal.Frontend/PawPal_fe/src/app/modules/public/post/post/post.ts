import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AnimalsHealthService } from '../../../../api-services/animals-health/animals-health-service';
import { GetAnimalsHealthByIdDto } from '../../../../api-services/animals-health/animals-health-model';
import { AnimalService } from '../../../../api-services/animals/animal';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { GetCityByIdDto } from '../../../../api-services/cities/cities.model';
import { AnimalUserService } from '../../../../api-services/animal-users/animal-users-service';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { environment } from '../../../../../environments/environment';
import { Carousel } from 'primeng/carousel';
import { ButtonPassThroughOptions } from 'primeng/button';
@Component({
  selector: 'app-post',
  standalone: false,
  templateUrl: './post.html',
  styleUrl: './post.scss',
  encapsulation: ViewEncapsulation.None,
})
export class PostComponent implements OnInit {
  route = inject(Router);
  animalId: number = 0;
  cityId: number = 0;
  userId: number = 0;
  postId: number = 0;
  animalHealthService = inject(AnimalsHealthService);
  animalService = inject(AnimalService);
  cityService = inject(CitiesService);
  userService = inject(AnimalUserService);
  postImageService = inject(PostImagesService);
  animalHealth: GetAnimalsHealthByIdDto = {
    animalHealthHistoryId: 0,
    animalId: 0,
    vaccinated: false,
    parasiteFree: false,
    spayedOrNeutered: false,
    dietaryRestrictions: '',
    animalAllergies: [],
    animalDisabilities: [],
  };
  animal = {
    name: '',
    category: '',
    breed: '',
    gender: '',
    age: 0,
    hasPapers: false,
  };
  user = {
    firstName: '',
    lastName: '',
    dateTime: '',
  };
  city: GetCityByIdDto = {
    id: 0,
    name: '',
  };
  postImage: any;
  dateAdded: string = '';
  imagesList: Array<string> = new Array<string>();
  env = environment;
  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.animalId = history.state.animalId;
    this.cityId = history.state.city;
    this.dateAdded = new Date(history.state.dateAdded).toLocaleDateString('en-UK');
    this.userId = history.state.userId;
    this.postId = history.state.postId;
    this.loadAnimal();
    this.loadHealth();
    this.loadCity();
    this.loadUsers();
    this.loadPostImages();
  }
  keepOrder = (a: any, b: any) => 0;
  loadHealth(): void {
    this.animalHealthService.getAnimalHealthHistoryById(this.animalId).subscribe((response) => {
      this.animalHealth = response;
    });
  }
  loadAnimal(): void {
    this.animalService.getAnimalById(this.animalId as number).subscribe((response) => {
      let sourceKeys = Object.keys(response);
      sourceKeys.forEach((key) => {
        if (key in this.animal) {
          (this.animal as any)[key] = (response as any)[key];
        }
      });
    });
  }
  loadCity(): void {
    this.cityService.getCityById(this.cityId).subscribe((response) => {
      this.city = response;
    });
  }
  loadUsers(): void {
    this.userService.getUser(this.userId).subscribe((response) => {
      this.user = response;
    });
  }
  loadPostImages(): void {
    this.postImageService.getImagePost(this.postId).subscribe((response) => {
      this.imagesList = response.postImages;
    });
  }
}
class PImage {
  imageUrl: string = '';
  constructor(url: string) {
    this.imageUrl = url;
  }
}
