import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { NgSelectModule } from '@ng-select/ng-select';
import { DatePicker } from 'primeng/datepicker';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { from } from 'rxjs';
import { CatalogComponent } from './modules/public/catalog/catalog/catalog';
@NgModule({
  declarations: [App],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgSelectModule,
    DatePicker,
    MatDatepickerModule,
    MatInputModule,
    MatNativeDateModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  providers: [provideBrowserGlobalErrorListeners(), provideHttpClient(withFetch())],
  bootstrap: [App],
})
export class AppModule {}
