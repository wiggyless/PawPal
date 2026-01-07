import { NgModule } from '@angular/core';
import { AuthRoutingModule } from './auth-routing-module';
import { LoginComponent } from './login/login/login';
import { AuthLayout } from './auth-layout/auth-layout/auth-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIcon } from "@angular/material/icon";
import { MatInput } from "@angular/material/input";
import { CommonModule } from '@angular/common';
import { LogoutComponent } from './logout/logout/logout';
@NgModule({
  declarations: [AuthLayout, LoginComponent, LogoutComponent],
  imports: [AuthRoutingModule, ReactiveFormsModule, MatIcon, MatInput, CommonModule],
})
export class AuthModule {}
