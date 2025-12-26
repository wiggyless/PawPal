import { Component, inject, OnInit } from '@angular/core';
import { AnimalCategoriesService } from '../../../api-services/animal-categories/animal-categories.service';
import { CitiesService } from '../../../api-services/cities/cities.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.scss',
})
export class PublicLayout implements OnInit {
  animalService = inject(AnimalCategoriesService);
  cityService = inject(CitiesService);
  routeLink: string = '';
  cities: any = [];
  animalCategories: any = [];

  ngOnInit(): void {
    this.loadCategories();
    this.loadCities();
  }
  loadCities() {
    let startFrom = new Date().getTime();
    this.cities = this.cityService.listCities().subscribe((response) => {
      this.cities = response;
      console.log(new Date().getTime() - startFrom); // for calculating response in ms
    });
  }

  loadCategories(): void {
    this.animalCategories = this.animalService.listAnimalCategories().subscribe((response) => {
      this.animalCategories = response;
    });
  }
}
