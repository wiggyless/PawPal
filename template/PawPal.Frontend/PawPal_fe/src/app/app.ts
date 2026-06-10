import { Component, signal } from '@angular/core';
import { initializeApp } from 'firebase/app';
import { environment } from '../environments/environment';
import { getAnalytics } from "firebase/analytics";


@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('PawPal_fe');

  constructor() {
    initializeApp(environment.firebase);
  }
}