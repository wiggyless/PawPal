import { platformBrowser } from '@angular/platform-browser';
import { AppModule } from './app/app-module';
import { initializeApp } from 'firebase/app';
import { environment } from './environments/environment.development';

initializeApp(environment.firebase);

platformBrowser()
  .bootstrapModule(AppModule, {
    ngZoneEventCoalescing: true,
  })
  .catch((err) => console.error(err));
