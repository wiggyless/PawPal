import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayout } from './admin-layout/admin-layout/admin-layout';
import { UserProfileComponent } from '../client/my-profile/user-profile-component/user-profile-component';
import { AdminProfile } from './admin-profile/admin-profile/admin-profile';
import { UserProfiles } from './user-profiles/user-profiles/user-profiles';
import { Profile } from './user-profiles/user-profiles/profile/profile/profile';
import { SettingsComponent } from '../client/settings/settings-component';
import { ReportedUsers } from './reported-users/reported-users/reported-users';
import { ReportedPosts } from './reported-posts/reported-posts/reported-posts';
import { ReportedComments } from './reported-comments/reported-comments/reported-comments';
import { DisabledUsers } from './disabled-users/disabled-users/disabled-users';
import { myAuthGuard } from '../../core/guards/my-auth-guard';
import { ReportedProblems } from './reported-problems/reported-problems/reported-problems';

const routes: Routes = [
  {
    canActivate: [myAuthGuard],
    data: { requireAuth: true, requireRoleId: 3 },
    path: '',
    component: AdminLayout,
    children: [
      { path: '', redirectTo: 'my-profile', pathMatch: 'full' },
      { path: 'user-profiles', component: UserProfiles },
      { path: 'my-profile', component: AdminProfile },
      { path: 'profile', component: Profile },
      { path: 'settings', component: SettingsComponent },
      { path: 'reported-users', component: ReportedUsers },
      { path: 'reported-posts', component: ReportedPosts },
      { path: 'reported-comments', component: ReportedComments },
      { path: 'disabled-users', component: DisabledUsers },
      { path: 'reported-problems', component: ReportedProblems },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
