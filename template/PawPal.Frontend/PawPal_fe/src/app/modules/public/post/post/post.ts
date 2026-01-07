import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AnimalsHealthService } from '../../../../api-services/animals-health/animals-health-service';
import { GetAnimalsHealthByIdDto } from '../../../../api-services/animals-health/animals-health-model';
import { AnimalService } from '../../../../api-services/animals/animal';
import { Observable } from 'rxjs';
import { GetAnimalByIdDto } from '../../../../api-services/animals/animal-model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { GetCityByIdDto } from '../../../../api-services/cities/cities.model';
@Component({
  selector: 'app-post',
  standalone: false,
  templateUrl: './post.html',
  styleUrl: './post.scss',
})
export class PostComponent implements OnInit {
  route = inject(Router);
  animalId: number = 0;
  animalHealthService = inject(AnimalsHealthService);
  animalService = inject(AnimalService);
  cityId: number = 0;
  cityService = inject(CitiesService);
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
  animal: GetAnimalByIdDto = {
    id: 0,
    name: '',
    breed: '',
    gender: '',
    category: '',
    age: 0,
    hasPapers: false,
    childFriendly: false,
  };
  city: GetCityByIdDto = {
    id: 0,
    name: '',
  };
  dateAdded: Date = new Date();
  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.animalId = history.state.animalId;
    this.cityId = history.state.city;
    this.dateAdded = history.state.dateAdded;
    this.loadAnimal();
    this.loadHealth();
    this.loadCity();
  }
  loadHealth(): void {
    this.animalHealthService.getAnimalHealthHistoryById(this.animalId).subscribe((response) => {
      this.animalHealth = response;
    });
  }
  loadAnimal(): void {
    this.animalService.getAnimalById(this.animalId as number).subscribe((response) => {
      this.animal = response;
    });
  }
  loadCity(): void {
    this.cityService.getCityById(this.cityId).subscribe((response) => {
      this.city = response;
    });
  }
  checkHealth() {
    console.log(this.animalHealth);
    console.log(this.animal);
    console.log(this.city);
  }
}
