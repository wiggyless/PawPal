import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserProfileComponent } from './my-profile/user-profile-component/user-profile-component';
import { ClientLayout } from './client-layout/client-layout/client-layout';
import { SharedModule } from 'primeng/api';
import { PublicLayout } from '../public/public-layout/public-layout';
const routes: Routes = [
  {
    path: '',
    component: UserProfileComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientRoutingModule {}
