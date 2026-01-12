import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublicLayout } from './public-layout/public-layout';
import { CatalogComponent } from './catalog/catalog/catalog';
import { PostComponent } from './post/post/post';
import { UserProfileComponent } from '../client/my-profile/user-profile-component/user-profile-component';
const routes: Routes = [
  {
    path: '',
    component: PublicLayout,
  },
  {
    path: 'catalog',
    component: CatalogComponent,
  },
  {
    path: 'post',
    component: PostComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PublicRoutingModule {}
