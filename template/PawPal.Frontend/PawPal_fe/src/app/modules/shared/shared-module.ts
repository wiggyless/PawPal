import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PublicRoutingModule } from './shared-routing-module';
import { NavbarComponent } from './components/navbar/navbar';
import { RouterOutlet } from '@angular/router';
import { ClientRoutingModule } from '../client/client-routing-module';
import { SessionWarningDialog } from './session/session-warning-dialog/session-warning-dialog';
@NgModule({
  declarations: [
    SessionWarningDialog
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    PublicRoutingModule,
    NavbarComponent,
    RouterOutlet,
  ],
  providers: [],
  exports: [CommonModule, ReactiveFormsModule, FormsModule, NavbarComponent],
})
export class SharedModule {}
