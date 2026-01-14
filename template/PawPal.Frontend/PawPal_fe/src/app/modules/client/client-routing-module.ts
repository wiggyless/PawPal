import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserProfileComponent } from './my-profile/user-profile-component/user-profile-component';
import { ClientLayout } from './client-layout/client-layout/client-layout';
import { SharedModule } from 'primeng/api';
import { PublicLayout } from '../public/public-layout/public-layout';
import { MyPosts } from './my-posts/my-posts';
import { CreatePost } from './create-post/create-post';
const routes: Routes = [
  {
    path: '',
    component: ClientLayout,
  },
  {
    path: 'create-post',
    component: CreatePost,
  },
  {
    path: 'myPosts',
    component: MyPosts,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientRoutingModule {}
