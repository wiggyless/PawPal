import {NgModule} from '@angular/core';
import { PublicRoutingModule } from './public-routing-module';
import { PublicLayout } from './public-layout/public-layout';
@NgModule({
  declarations: [  
    PublicLayout
  ],
  imports: [
    PublicRoutingModule,
  ]
})
export class PublicModule { }