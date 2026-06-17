import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { myAuthGuard } from './core/guards/my-auth-guard';
import { Adoption } from './modules/client/adpotion/adoption/adoption';
import { LoginComponent } from './modules/auth/login/login/login';
import { Messaging } from './modules/client/messaging/messaging';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./modules/public/public-module').then((m) => m.PublicModule),
  },
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
  
  {path: 'client/messaging',
    component: Messaging,
    loadChildren: () => ClientModule,
  },
  {
    path: 'client', 
    component: PublicLayout,
    loadChildren: () => PublicModule,
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
