import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientLayout } from './client-layout/client-layout/client-layout';
import { MyPosts } from './my-posts/my-posts';
import { CreatePost } from './create-post/create-post';
import { MyRequests } from './my-requests/my-requests/my-requests';
import { MyRequestsDialog } from './my-requests/my-requests-dialog/my-requests-dialog/my-requests-dialog';
import { MyFavorites } from './my-favorites/my-favorites/my-favorites';
import { SettingsComponent } from './settings/settings-component';
import { MySentRequests } from './my-sent-requests/my-sent-requests/my-sent-requests';
import { RequestHistory } from './request-history/request-history/request-history';
import { Messaging } from './messaging/messaging';
import { UserProfileComponent } from './my-profile/user-profile-component/user-profile-component';
import { Adoption } from './adpotion/adoption/adoption';
const routes: Routes = [
  {
    path: '',
    component: ClientLayout, // this should contain <router-outlet>
    children: [
      { path: 'myPosts', component: MyPosts },
      { path: 'my-profile', component: UserProfileComponent },
      { path: 'my-requests', component: MyRequests },
      { path: 'my-sent-requests', component: MySentRequests },
      { path: 'my-requests-dialog', component: MyRequestsDialog },
      { path: 'request-history', component: RequestHistory },
      { path: 'my-favorties', component: MyFavorites }, // fixed typo
      { path: 'settings', component: SettingsComponent },
    ],
  },
  {
    path: 'messages', 
    component: Messaging,
  },
   { path: 'create-post', component: CreatePost },
   {path: 'adoption', component:Adoption}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientRoutingModule {}
