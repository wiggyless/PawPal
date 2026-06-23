import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { myAuthGuard } from './core/guards/my-auth-guard';
import { PreloadDashboardStrategy } from './core/preload/preload-strategy';
const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./modules/public/public-module').then((m) => m.PublicModule),
  },
  {
    path: 'admin',
    canActivate: [myAuthGuard],
    data: { requireAuth: true, requireRoleId: 3 },
    loadChildren: () => import('./modules/admin/admin-module').then((m) => m.AdminModule),
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth-module').then((m) => m.AuthModule),
  },
  {
    path: 'client',
    canActivate: [myAuthGuard],
    data: { requireAuth: true, path: 'client' },
    loadChildren: () => import('./modules/client/client-module').then((m) => m.ClientModule),
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadDashboardStrategy })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
