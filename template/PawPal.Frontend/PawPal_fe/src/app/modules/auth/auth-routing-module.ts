import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthLayout } from './auth-layout/auth-layout/auth-layout';
import { LoginComponent } from './login/login/login';
import { PublicLayout } from '../public/public-layout/public-layout';
import { LogoutComponent } from './logout/logout/logout';
import { ClientLayout } from '../client/client-layout/client-layout/client-layout';

const routes: Routes = [
  {
    path: '',
    component: AuthLayout,
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
      { path: 'logout', component: LogoutComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
