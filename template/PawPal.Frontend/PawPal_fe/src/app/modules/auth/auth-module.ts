import { NgModule } from '@angular/core';
import { AuthRoutingModule } from './auth-routing-module';
import { LoginComponent } from './login/login/login';
import { AuthLayout } from './auth-layout/auth-layout/auth-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIcon, MatIconModule } from "@angular/material/icon";
import { MatInput, MatFormField, MatLabel, MatHint, MatSuffix } from "@angular/material/input";
import { CommonModule } from '@angular/common';
import { LogoutComponent } from './logout/logout/logout';
import { RegisterComponent } from './register/register-component/register-component';
import {MatStepperModule} from '@angular/material/stepper';
import { NgSelectComponent, NgOptionComponent } from "@ng-select/ng-select";
import { MatDatepicker, MatDatepickerToggle } from '@angular/material/datepicker';

import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinner } from "@angular/material/progress-spinner";
@NgModule({
  declarations: [AuthLayout, LoginComponent, LogoutComponent, RegisterComponent],
  imports: [AuthRoutingModule,
    ReactiveFormsModule,
    MatIcon,
    MatInput,
    CommonModule,
    MatStepperModule,
    NgSelectComponent,
    NgOptionComponent,
    MatDatepicker,
    MatFormField,
    MatLabel,
    MatDatepickerToggle,
    MatHint,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule, MatSuffix, MatProgressSpinner],

})
export class AuthModule {}
