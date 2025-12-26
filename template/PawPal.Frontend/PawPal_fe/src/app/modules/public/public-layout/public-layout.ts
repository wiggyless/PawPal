import { Component, inject, OnInit } from '@angular/core';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { CitiesService } from '../../../api-services/cities/cities.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.scss',
})
export class PublicLayout implements OnInit{

  animalService = inject(AnimalCategoriesService);
  cityService = inject(CitiesService);
  currentUser = inject(CurrentUserService);
  
  cities : any = [];
  animalCategories : any = [];

  ngOnInit(): void {
    this.loadCategories();
    this.loadCities();
  }
  loadCities() {
    this.cities = this.cityService.listCities().subscribe(response => {
      this.cities=response;
    });
  }
  
  loadCategories() : void{
 this.animalCategories = this.animalService.listAnimalCategories().subscribe(response => {
        this.animalCategories  =response;});
  }
  
}

