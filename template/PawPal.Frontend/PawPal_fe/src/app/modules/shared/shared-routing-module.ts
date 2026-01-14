import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../auth/login/login/login';
import { PublicLayout } from '../public/public-layout/public-layout';
import { UserProfileComponent } from '../client/my-profile/user-profile-component/user-profile-component';
import { ClientModule } from '../client/client-module';
import { ClientLayout } from '../client/client-layout/client-layout/client-layout';
import { PostComponent } from '../public/post/post/post';
import { ClientRoutingModule } from '../client/client-routing-module';

const routes: Routes = [
  {
    path: '',
    component: PublicLayout,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PublicRoutingModule {}
