import { NgModule } from '@angular/core';
import { PublicRoutingModule } from './public-routing-module';
import { PublicLayout } from './public-layout/public-layout';
import { CatalogComponent } from './catalog/catalog';
@NgModule({
  declarations: [PublicLayout, CatalogComponent],
  imports: [PublicRoutingModule],
})
export class PublicModule {}
