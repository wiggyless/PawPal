import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublicLayout } from './public-layout/public-layout';
import { CatalogComponent } from './catalog/catalog/catalog';
import { NavbarComponent } from '../shared/components/navbar/navbar';
const routes: Routes = [
  {
    path: '',
    component: PublicLayout,
  },
  {
    path: 'catalog',
    component: CatalogComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PublicRoutingModule {}
