import { NgModule } from '@angular/core';
import { PublicRoutingModule } from './public-routing-module';
import { PublicLayout } from './public-layout/public-layout';
import { NavbarComponent } from '../shared/components/navbar/navbar';
@NgModule({
  declarations: [  
    PublicLayout,
    NavbarComponent
  ],
  imports: [
    PublicRoutingModule,
  ]
})
export class PublicModule {}
