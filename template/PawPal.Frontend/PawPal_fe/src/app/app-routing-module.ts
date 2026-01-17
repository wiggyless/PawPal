import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientLayout } from './modules/client/client-layout/client-layout/client-layout';
import { NavbarComponent } from './modules/shared/components/navbar/navbar';
import { UserProfileComponent } from './modules/client/my-profile/user-profile-component/user-profile-component';
import { PublicLayout } from './modules/public/public-layout/public-layout';
import { ClientModule } from './modules/client/client-module';
import { SharedModule } from 'primeng/api';
import { PublicModule } from './modules/public/public-module';
import { CreatePost } from './modules/client/create-post/create-post';
import {  myAuthGuard } from './core/guards/my-auth-guard';

const routes: Routes = [
  {
    path: 'admin',
    canActivate: [myAuthGuard],
    data: { requireAuth: true, requireRoleId: 3 }, 
    loadChildren: () => import('./modules/public/public-module').then((m) => m.PublicModule),
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth-module').then((m) => m.AuthModule),
  },
  {
    path: 'client', // bilo ko logiran
    component: PublicLayout,
    loadChildren: () => PublicModule,
  },
  {
    path: 'client/my-profile',
    loadChildren: () => ClientModule,
  },
  {
    path: 'client/create-post',
    component: CreatePost,
    loadChildren: () => ClientModule,
  },
  {
    path: '',
    loadChildren: () => import('./modules/public/public-module').then((m) => m.PublicModule),
  },
  // fallback 404
  //{ path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
