import { platformBrowser } from '@angular/platform-browser';
import { AppModule } from './app/app-module';
import { environment } from './environments/environment';
import { initializeApp } from 'firebase/app';

initializeApp(environment.firebase);

initializeApp(environment.firebase);

platformBrowser()
  .bootstrapModule(AppModule, {
    ngZoneEventCoalescing: true,
  })
  .catch((err) => console.error(err));
