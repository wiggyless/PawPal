import { NgModule } from '@angular/core';
import { AuthRoutingModule } from './auth-routing-module';
import { LoginComponent } from './login/login/login';
import { AuthLayout } from './auth-layout/auth-layout/auth-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIcon } from "@angular/material/icon";
import { MatInput } from "@angular/material/input";
@NgModule({
  declarations: [AuthLayout, LoginComponent],
  imports: [AuthRoutingModule, ReactiveFormsModule, MatIcon, MatInput],
})
export class AuthModule {}
