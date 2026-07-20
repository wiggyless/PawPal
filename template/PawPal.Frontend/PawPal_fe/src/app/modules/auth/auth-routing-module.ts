import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthLayout } from './auth-layout/auth-layout/auth-layout';
import { LoginComponent } from './login/login';
import { LogoutComponent } from './logout/logout';
import { ClientLayout } from '../client/client-layout/client-layout/client-layout';
import { RegisterComponent } from './register/register-component/register-component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email';
import { ConfirmEmailChangeComponent } from './confirm-email-change/confirm-email-change';
import { PublicLayout } from '../public/public-layout/public-layout';
import { PreloadDashboardStrategy } from '../../core/preload/preload-strategy';

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
      { path: 'register', component: RegisterComponent },
      { path: 'confirm-email', component: ConfirmEmailComponent },
      { path: 'confirm-email-change', component: ConfirmEmailChangeComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
