import { NgModule } from '@angular/core';
import { RouterModule, Routes, RouterLink } from '@angular/router';
import { LoginComponent } from '../auth/login/login/login';
import { PublicLayout } from '../public/public-layout/public-layout';

const routes: Routes = [
  {
    path: '',
    component: PublicLayout,
  },
  {
    path: '/auth/login',
    component: LoginComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PublicRoutingModule {}
