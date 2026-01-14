import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthLayout } from './auth-layout/auth-layout/auth-layout';
import { LoginComponent } from './login/login/login';
import { LogoutComponent } from './logout/logout/logout';
import { ClientLayout } from '../client/client-layout/client-layout/client-layout';
import { RegisterComponent } from './register/register-component/register-component';

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
      {path: 'register', component:RegisterComponent}
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
