import { NgModule } from '@angular/core';
import { ClientRoutingModule } from './client-routing-module';
import { UserProfileComponent } from './my-profile/user-profile-component/user-profile-component';
import { ClientLayout } from './client-layout/client-layout/client-layout';
import { NavbarComponent } from '../shared/components/navbar/navbar';
import { RouterOutlet } from '@angular/router';
import { SharedModule } from 'primeng/api';
@NgModule({
  declarations: [UserProfileComponent, ClientLayout],
  imports: [ClientRoutingModule, NavbarComponent, RouterOutlet],
})
export class ClientModule {}
