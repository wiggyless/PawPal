import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayout } from './admin-layout/admin-layout/admin-layout';
import { UserProfileComponent } from '../client/my-profile/user-profile-component/user-profile-component';
import { AdminProfile } from './admin-profile/admin-profile/admin-profile';
import { UserProfiles } from './user-profiles/user-profiles/user-profiles';
import { Profile } from './user-profiles/user-profiles/profile/profile/profile';
import { SettingsComponent } from '../client/settings/settings-component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayout,
    children: [
      { path: 'user-profiles', component: UserProfiles },
      { path: 'my-profile', component: AdminProfile },
      { path: 'profile', component: Profile },
      { path: 'settings', component: SettingsComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
