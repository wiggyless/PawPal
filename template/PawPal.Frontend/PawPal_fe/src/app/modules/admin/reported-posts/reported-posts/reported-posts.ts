import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Subscription, forkJoin, take } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { GetMainImagePostBlobClass } from '../../../../api-services/animal-post-images/animal-post-images-model';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';
import {
  GetUserReportDto,
  REPORT_REASON_USER_LABELS,
  GetUserReportQuery,
} from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.model';
import { ReportUserService } from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.service';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PageResult } from '../../../../core/models/paging/page-result';
import { GetReportedPostDto } from '../../../../api-services/moderation/reported-posts/reported-posts.model';
import { ReportedPostsService } from '../../../../api-services/moderation/reported-posts/reported-posts.service';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-reported-posts',
  standalone: false,
  templateUrl: './reported-posts.html',
  styleUrl: './reported-posts.scss',
})
export class ReportedPosts implements OnInit, OnDestroy {
  currentUser = inject(CurrentUserService);
  postReportService = inject(ReportedPostsService);
  envLink = environment;
  isLoaded = false;
  private mySubscription?: Subscription;
  private myPostSubscription?: Subscription;
  requestsList: PageResult<GetReportedPostDto> | undefined;
  fullName: string = '';
  dialog = inject(MatDialog);
  imagesLoaded = signal(false);
  tempList: number[] = [];
  sanitizer = inject(DomSanitizer);
  reasons = REPORT_REASON_USER_LABELS;
  router = inject(Router);
  request: GetUserReportQuery = {
    paging: {
      page: 1,
      pageSize: 4,
    },
  };
  cd = inject(ChangeDetectorRef);
  dialoguePopup = inject(DialoguePopupService);
  datePicker = new FormGroup({
    start: new FormControl<any>(null),
    end: new FormControl<any>(null),
  });
  ngOnInit(): void {
    this.loadRequest();
  }
  loadRequest() {
    this.request.dateSentMax = this.datePicker.value.end;
    this.request.dateSentMin = this.datePicker.value.start;
    this.myPostSubscription = forkJoin({
      request: this.postReportService.getPostReportList(this.request),
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
  routeToPost(id: number) {
    this.router.navigate(['/post'], {
      queryParams: {
        postID: id,
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
        this.postReportService
          .deletePostReport(id)
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
  getDetails(request: GetReportedPostDto) {
    this.dialoguePopup.info(
      'Reason: ' + this.reasons[request.reason].label,
      ('Description: ' + request.description) as string,
      'OK',
    );
  }
}
