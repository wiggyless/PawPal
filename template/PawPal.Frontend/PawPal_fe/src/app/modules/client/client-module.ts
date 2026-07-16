import { NgModule } from '@angular/core';
import { ClientRoutingModule } from './client-routing-module';
import { UserProfileComponent } from './my-profile/user-profile-component/user-profile-component';
import { ClientLayout } from './client-layout/client-layout/client-layout';
import { NavbarComponent } from '../shared/components/navbar/navbar';
import { RouterOutlet } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MyPosts } from './my-posts/my-posts';
import { UserProfileSideNavbar } from './user-profile-side-navbar/user-profile-side-navbar/user-profile-side-navbar';
import { MatListModule } from '@angular/material/list';
import { AsyncPipe, CommonModule, DatePipe, KeyValuePipe } from '@angular/common';
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
import { MyFavorites } from './my-favorites/my-favorites/my-favorites';
import { MyRequests } from './my-requests/my-requests/my-requests';
import { MyRequestsDialog } from './my-requests/my-requests-dialog/my-requests-dialog/my-requests-dialog';
import { SettingsComponent } from './settings/settings-component';
import { Adoption } from './adpotion/adoption/adoption';
import { MySentRequests } from './my-sent-requests/my-sent-requests/my-sent-requests';
import { MySentRequestDialog } from './my-sent-requests/my-sent-request-dialog/my-sent-request-dialog/my-sent-request-dialog';
import { RequestHistory } from './request-history/request-history/request-history';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { UserProfileImageCropDialog } from './my-profile/user-profile-component/user-profile-imageCrop/user-profile-image-crop-dialog/user-profile-image-crop-dialog';
import { SecurityQuestionsDialog } from './settings/securityQuestions-dialog/security-questions-dialog/security-questions-dialog';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatSpinner } from '@angular/material/progress-spinner';
import { Messaging } from './messaging/messaging';
import { SlicePipe, UpperCasePipe } from '@angular/common';
import { ImageCropperComponent } from 'ngx-image-cropper';
import { MatSelect } from '@angular/material/select';
import { ReportPostComponent } from './report-post-component/report-post-component';
import { ReportUserComponent } from './report-user-component/report-user-component/report-user-component';
import { ReportCommentComponent } from './report-comment-component/report-comment-component/report-comment-component';

@NgModule({
  declarations: [
    UserProfileComponent,
    ClientLayout,
    MyPosts,
    UserProfileSideNavbar,
    CreatePost,
    DialogueComponent,
    MyFavorites,
    MyRequests,
    MyRequestsDialog,
    SettingsComponent,
    Adoption,
    MySentRequests,
    MySentRequestDialog,
    RequestHistory,
    UserProfileImageCropDialog,
    SecurityQuestionsDialog,
    Messaging,
    ReportPostComponent,
    ReportUserComponent,
    ReportCommentComponent,
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
    MatIcon,
    AsyncPipe,
    MatPaginator,
    MatDialogModule,
    A11yModule,
    DatePipe,
    MatProgressBarModule,
    DragDropModule,
    MatSpinner,
    SlicePipe,
    UpperCasePipe,
    ImageCropperComponent,
    MatSelect,
    CommonModule,
  ],
  exports: [ReportPostComponent],
  providers: [provideNativeDateAdapter(), MatDialog],
})
export class ClientModule {}
