import {NgModule} from '@angular/core';
import { AuthRoutingModule } from './auth-routing-module';
import { LoginComponent } from './login/login/login';
import { AuthLayout } from './auth-layout/auth-layout/auth-layout';
import { ReactiveFormsModule } from '@angular/forms';
@NgModule({
  declarations: [  
    AuthLayout, LoginComponent
  ],
  imports: [
    AuthRoutingModule,
    ReactiveFormsModule,
  ]
})
export class AuthModule { }