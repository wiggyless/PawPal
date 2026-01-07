import { NgModule } from '@angular/core';
import { PublicRoutingModule } from './public-routing-module';
import { PublicLayout } from './public-layout/public-layout';
import { CatalogComponent } from './catalog/catalog/catalog';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { DatePicker } from 'primeng/datepicker';
import { MatFormField, MatLabel, MatInput } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { NavbarComponent } from '../shared/components/navbar/navbar';
import { PostComponent } from './post/post/post';
@NgModule({
  declarations: [PublicLayout, CatalogComponent, PostComponent],
  imports: [
    NavbarComponent,
    PublicRoutingModule,
    FormsModule,
    NgSelectModule,
    DatePicker,
    MatFormField,
    MatLabel,
    MatInput,
    MatDatepickerModule,
    MatInputModule,
    MatNativeDateModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule,
  ],
})
export class PublicModule {}
