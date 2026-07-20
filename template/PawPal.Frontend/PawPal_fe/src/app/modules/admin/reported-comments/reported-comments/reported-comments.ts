import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Subscription, forkJoin, take } from 'rxjs';
import { environment } from '../../../../../environments/environment.development';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';
import { GetUserReportQuery } from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.model';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PageResult } from '../../../../core/models/paging/page-result';
import {
  GetReportedCommentDto,
  REPORT_REASON_COMMENTS_LABELS,
} from '../../../../api-services/moderation/reported-posts/reported-comments/reported-comments.model';
import { ReportCommentService } from '../../../../api-services/moderation/reported-posts/reported-comments/reported-comments.service';
import { ReportedCommentDialog } from './reported-comment-dialog/reported-comment-dialog/reported-comment-dialog';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-reported-comments',
  standalone: false,
  templateUrl: './reported-comments.html',
  styleUrl: './reported-comments.scss',
})
export class ReportedComments implements OnInit, OnDestroy {
  currentUser = inject(CurrentUserService);
  commentReportService = inject(ReportCommentService);
  envLink = environment;
  isLoaded = false;
  private mySubscription?: Subscription;
  private myPostSubscription?: Subscription;
  requestsList: PageResult<GetReportedCommentDto> | undefined;
  fullName: string = '';
  dialog = inject(MatDialog);
  imagesLoaded = signal(false);
  tempList: number[] = [];
  sanitizer = inject(DomSanitizer);
  reasons = REPORT_REASON_COMMENTS_LABELS;
  router = inject(Router);
  request: GetUserReportQuery = {
    paging: {
      page: 1,
      pageSize: 4,
    },
  };
  datePicker = new FormGroup({
    start: new FormControl<any>(null),
    end: new FormControl<any>(null),
  });
  cd = inject(ChangeDetectorRef);
  dialoguePopup = inject(DialoguePopupService);
  ngOnInit(): void {
    this.loadRequest();
  }
  loadRequest() {
    this.request.dateSentMax = this.datePicker.value.end;
    this.request.dateSentMin = this.datePicker.value.start;
    this.myPostSubscription = forkJoin({
      request: this.commentReportService.getCommentReportList(this.request),
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
        this.commentReportService
          .deleteCommentReport(id)
          .pipe(take(1))
          .subscribe({
            next: (res) => {
              this.dialoguePopup.success('Success', 'Comment has been succesfully dissmised', 'OK');
              this.imagesLoaded.set(false);
              this.cd.detectChanges();
              this.loadRequest();
            },
            error: (res) => {
              this.dialoguePopup.error('Error', 'An error has occurred, please try again', 'OK');
            },
          });
      },
      () => {},
    );
  }
  getDetails(request: GetReportedCommentDto) {
    this.dialog
      .open(ReportedCommentDialog, {
        minWidth: '1200px',
        maxWidth: '1500px',
        panelClass: 'custom-dialog-bg',
        hasBackdrop: true,
        data: {
          comment: request.comment,
          request: request,
        },
      })
      .afterClosed()
      .subscribe((res) => {
        if (res) {
          this.imagesLoaded.set(false);
          this.cd.detectChanges();
          this.loadRequest();
        }
      });
  }
}
