import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Subscription, forkJoin, take } from 'rxjs';
import { environment } from '../../../../../environments/environment.development';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';
import { GetReportedPostDto } from '../../../../api-services/moderation/reported-posts/reported-posts.model';
import { ReportedPostsService } from '../../../../api-services/moderation/reported-posts/reported-posts.service';
import {
  REPORT_REASON_USER_LABELS,
  GetUserReportQuery,
} from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.model';
import { PageResult } from '../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { ReportedProblemService } from '../../../../api-services/moderation/reported-problem/reported-problem.service';
import { ReportedProblemsDto } from '../../../../api-services/moderation/reported-problem/reported-problem.model';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-reported-problems',
  standalone: false,
  templateUrl: './reported-problems.html',
  styleUrl: './reported-problems.scss',
})
export class ReportedProblems implements OnInit, OnDestroy {
  currentUser = inject(CurrentUserService);
  problemReportService = inject(ReportedProblemService);
  envLink = environment;
  isLoaded = false;
  private mySubscription?: Subscription;
  private myPostSubscription?: Subscription;
  requestsList: PageResult<ReportedProblemsDto> | undefined;
  fullName: string = '';
  dialog = inject(MatDialog);
  imagesLoaded = signal(false);
  tempList: number[] = [];
  sanitizer = inject(DomSanitizer);
  router = inject(Router);
  request: GetUserReportQuery = {
    paging: {
      page: 1,
      pageSize: 10,
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
      request: this.problemReportService.getProblemReportList(this.request),
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
        this.problemReportService
          .deleteProblemReport(id)
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
  getDetails(request: ReportedProblemsDto) {
    this.dialoguePopup.info(('Description: ' + request.description) as string, 'OK');
  }
}
