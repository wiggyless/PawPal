import { Component, signal } from '@angular/core';
import { AnimalService } from './api-services/animals/animal';
import { inject } from '@angular/core';
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('PawPal_fe');
  animalService = inject(AnimalService);
  _animals: any[] = [];

  constructor(){
    this.animalService.get().subscribe(animals=>{
      console.log(animals);
      this._animals = animals.items;
    })
  }
}
