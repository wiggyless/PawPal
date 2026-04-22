import { NgModule } from '@angular/core';
import { ClientRoutingModule } from './client-routing-module';
import { UserProfileComponent } from './my-profile/user-profile-component/user-profile-component';
import { ClientLayout } from './client-layout/client-layout/client-layout';
import { NavbarComponent } from '../shared/components/navbar/navbar';
import { RouterOutlet } from '@angular/router';
import { SharedModule } from 'primeng/api';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MyPosts } from './my-posts/my-posts';
import { UserProfileSideNavbar } from './user-profile-side-navbar/user-profile-side-navbar/user-profile-side-navbar';
import { MatListModule } from '@angular/material/list';
import { AsyncPipe, DatePipe, KeyValuePipe } from '@angular/common';
import { CreatePost } from './create-post/create-post';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule, MatDateRangeInput } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CarouselModule } from 'primeng/carousel';
import { MatIcon } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DialogueComponent } from './dialogue-component/dialogue-component';
import { MatPaginator } from '@angular/material/paginator';
import { A11yModule } from '@angular/cdk/a11y';
import { SaveChangesComponent } from './my-profile/user-profile-component/save-changes-component/save-changes-component';
import { MyFavorites } from './my-favorites/my-favorites/my-favorites';
import { MatSpinner } from '@angular/material/progress-spinner';
import { MyRequests } from './my-requests/my-requests/my-requests';
import { MyRequestsDialog } from './my-requests/my-requests-dialog/my-requests-dialog/my-requests-dialog';
import { SettingsComponent } from './settings/settings-component';
import { Adoption } from './adpotion/adoption/adoption';
import { MySentRequests } from './my-sent-requests/my-sent-requests/my-sent-requests';
import { MySentRequestDialog } from './my-sent-requests/my-sent-request-dialog/my-sent-request-dialog/my-sent-request-dialog';
import { RequestHistory } from './request-history/request-history/request-history';
@NgModule({
  declarations: [
    UserProfileComponent,
    ClientLayout,
    MyPosts,
    UserProfileSideNavbar,
    CreatePost,
    DialogueComponent,
    SaveChangesComponent,
    MyFavorites,
    MyRequests,
    MyRequestsDialog,
    SettingsComponent,
    Adoption,
    MySentRequests,
    MySentRequestDialog,
    RequestHistory,
  ],
  imports: [
    ClientRoutingModule,
    NavbarComponent,
    RouterOutlet,
    ReactiveFormsModule,
    MatListModule,
    KeyValuePipe,
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatGridListModule,
    MatSelectModule,
    FormsModule,
    MatCheckboxModule,
    MatDatepickerModule,
    CarouselModule,
    MatSelectModule,
    MatIcon,
    AsyncPipe,
    MatPaginator,
    MatDatepickerModule,
    MatDateRangeInput,
    MatDialogModule,
    A11yModule,
    MatSpinner,
    DatePipe,
  ],
  providers: [provideNativeDateAdapter(), MatDialog],
})
export class ClientModule {}
