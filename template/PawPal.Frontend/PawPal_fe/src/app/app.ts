import { Component, signal } from '@angular/core';
import { AnimalService } from './api-services/animals/animal';
import { inject } from '@angular/core';
import { AuthApiService } from './api-services/auth/auth-api.service'
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('PawPal_fe');
  loginService = inject(AuthApiService);
  //to-do

}
