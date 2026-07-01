import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { Subscription, forkJoin, take } from 'rxjs';
import { environment } from '../../../../../environments/environment.development';
import { GetMainImagePostBlobClass } from '../../../../api-services/animal-post-images/animal-post-images-model';
import { PageResult } from '../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { MyRequestsDialog } from '../../../client/my-requests/my-requests-dialog/my-requests-dialog/my-requests-dialog';
import { ReportUserService } from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.service';
import {
  GetUserReportDto,
  GetUserReportQuery,
  REPORT_REASON_USER_LABELS,
} from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.model';
import { Router, RouterLink } from '@angular/router';
import { PageEvent } from '@angular/material/paginator';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-reported-users',
  standalone: false,
  templateUrl: './reported-users.html',
  styleUrl: './reported-users.scss',
})
export class ReportedUsers implements OnInit, OnDestroy {
  currentUser = inject(CurrentUserService);
  userReportsService = inject(ReportUserService);
  envLink = environment;
  isLoaded = false;
  private mySubscription?: Subscription;
  private myPostSubscription?: Subscription;
  requestsList: PageResult<GetUserReportDto> | undefined;
  fullName: string = '';
  dialog = inject(MatDialog);
  catalogImages: GetMainImagePostBlobClass[] = [];
  imagesLoaded = signal(false);
  tempList: number[] = [];
  sanitizer = inject(DomSanitizer);
  reasons = REPORT_REASON_USER_LABELS;
  router = inject(Router);
  request: GetUserReportQuery = {
    paging: {
      page: 1,
      pageSize: 10,
    },
  };
  username = '';
  cd = inject(ChangeDetectorRef);
  datePicker = new FormGroup({
    start: new FormControl<any>(null),
    end: new FormControl<any>(null),
  });
  dialoguePopup = inject(DialoguePopupService);
  ngOnInit(): void {
    this.loadRequest();
  }
  loadRequest() {
    this.request.dateSentMax = this.datePicker.value.end;
    this.request.dateSentMin = this.datePicker.value.start;
    this.request.username = this.username;
    this.myPostSubscription = forkJoin({
      request: this.userReportsService.getUserReportList(this.request),
    }).subscribe({
      next: (response) => {
        this.requestsList = response.request;
        this.imagesLoaded.set(true);
      },
    });
  }
  ngOnDestroy(): void {
    this.mySubscription?.unsubscribe();
    this.myPostSubscription?.unsubscribe();
  }
  routeToProfile(request: GetUserReportDto) {
    this.router.navigate(['/admin/profile'], {
      queryParams: {
        userID: request.userReportedID,
        repID: request.reportID,
      },
    });
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadRequest();
  }

  dismiss(id: number) {
    this.dialoguePopup.warning(
      'Confirm action',
      'Are you sure that you want to dissmis this report?',
      'Yes',
      'No',
      () => {
        this.userReportsService
          .deleteUserReport(id)
          .pipe(take(1))
          .subscribe((res) => {
            this.dialoguePopup.success('Success', 'Report has been succesfully dissmised', 'OK');
            this.imagesLoaded.set(false);
            this.cd.detectChanges();
            this.loadRequest();
          });
      },
      () => {},
    );
  }
  getDetails(request: GetUserReportDto) {
    this.dialoguePopup.info(
      'Reason: ' + this.reasons[request.reason].label,
      ('Description: ' + request.description) as string,
      'OK',
    );
  }
}
