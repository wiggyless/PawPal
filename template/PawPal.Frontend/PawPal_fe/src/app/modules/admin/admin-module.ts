import { NgModule } from '@angular/core';
import { AdminRoutingModule } from './admin-routing-module';
import { AdminLayout } from './admin-layout/admin-layout/admin-layout';
import { AdminSidenavbar } from './admin-sidenavbar/admin-sidenavbar/admin-sidenavbar';
import { NavbarComponent } from '../shared/components/navbar/navbar';
import { MatListModule, MatNavList } from '@angular/material/list';
import { AsyncPipe, DatePipe, KeyValuePipe } from '@angular/common';
import { UserProfileComponent } from '../client/my-profile/user-profile-component/user-profile-component';
import { A11yModule } from '@angular/cdk/a11y';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule, MatDateRangeInput } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSpinner } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatStepperModule } from '@angular/material/stepper';
import { RouterOutlet } from '@angular/router';
import { ImageCropperComponent } from 'ngx-image-cropper';
import { CarouselModule } from 'primeng/carousel';
import { AdminProfile } from './admin-profile/admin-profile/admin-profile';
import { DateAdapter } from '@angular/material/core';
import { UserProfiles } from './user-profiles/user-profiles/user-profiles';
import { Profile } from './user-profiles/user-profiles/profile/profile/profile';
import { SettingsComponent } from '../client/settings/settings-component';
@NgModule({
  declarations: [AdminLayout, AdminSidenavbar, AdminProfile, UserProfiles, Profile],
  imports: [
    AdminRoutingModule,
    NavbarComponent,
    MatNavList,
    KeyValuePipe,
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
    DatePipe,
    MatProgressBarModule,
    ImageCropperComponent,
    DragDropModule,
    MatSpinner,
  ],
})
export class AdminModule {}
