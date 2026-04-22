import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserProfileComponent } from './my-profile/user-profile-component/user-profile-component';
import { ClientLayout } from './client-layout/client-layout/client-layout';
import { SharedModule } from 'primeng/api';
import { PublicLayout } from '../public/public-layout/public-layout';
import { MyPosts } from './my-posts/my-posts';
import { CreatePost } from './create-post/create-post';
import { MyRequests } from './my-requests/my-requests/my-requests';
import { MyRequestsDialog } from './my-requests/my-requests-dialog/my-requests-dialog/my-requests-dialog';
import { MyFavorites } from './my-favorites/my-favorites/my-favorites';
import { SettingsComponent } from './settings/settings-component';
import { MySentRequests } from './my-sent-requests/my-sent-requests/my-sent-requests';
import { RequestHistory } from './request-history/request-history/request-history';
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
  {
    path: 'my-requests',
    component: MyRequests,
  },
  {
    path: 'my-sent-requests',
    component: MySentRequests,
  },
  {
    path: 'my-requests-dialog',
    component: MyRequestsDialog,
  },
  {
    path: 'request-history',
    component: RequestHistory,
  },
  {
    path: 'my-favorties',
    component: MyFavorites,
  },
  {
    path: 'settings',
    component: SettingsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientRoutingModule {}
