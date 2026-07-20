import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  inject,
  Input,
  OnDestroy,
  OnInit,
  Output,
  signal,
  ViewChild,
} from '@angular/core';
import { SignalRService } from '../../../../../../core/services/signalr.service';
import { CommentService } from '../../../../../../api-services/comments/comments.service';
import {
  CommentDto,
  CreateCommentCommand,
  CommentQuery,
} from '../../../../../../api-services/comments/comments.model';
import { PageResult } from '../../../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../../../core/services/auth/current-user.service';
import { UserImageService } from '../../../../../../api-services/userImage/userImage-service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { environment } from '../../../../../../../environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { ReportCommentComponent } from '../../../../../client/report-comment-component/report-comment-component/report-comment-component';

@Component({
  selector: 'app-post-comments',
  standalone: false,
  templateUrl: './post-comments.html',
  styleUrl: './post-comments.scss',
})
export class PostComments implements OnInit, OnDestroy {
  @Input({ required: true }) postId!: number;
  @Output() commentsLoaded = new EventEmitter<void>();
  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  private signalRSubscription: Subscription = new Subscription();
  commentsService = inject(CommentService);
  currentUser = inject(CurrentUserService);
  userImgService = inject(UserImageService);
  router = inject(Router);
  cd = inject(ChangeDetectorRef);

  commentsList: PageResult<CommentDto> = this.createEmptyPageResult();
  hasLoaded = signal(false);
  isLoadingMore: boolean = false;
  comment = signal('');

  request: CommentQuery = {
    postID: 0,
    paging: {
      page: 1,
      pageSize: 10,
    },
  };
  env = environment.apiUrl;
  counter: number = 0;
  dialog = inject(MatDialog);
  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.request.postID = this.postId;
    this.loadComments();

    this.signalRSubscription = this.signalRService.commentReceived$.subscribe((newComment) => {
      if (newComment && newComment.postID == this.postId) {
        this.prependSignalRComment(newComment);
      }
    });
  }

  ngOnDestroy(): void {
    this.signalRSubscription.unsubscribe();
  }

  loadComments() {
    const isFirstLoad = !this.hasLoaded();
    this.commentsService.getComments(this.request).subscribe({
      next: (incomingData) => {
        const existingItems = this.commentsList?.items || [];
        const newItems = incomingData.items || [];

        this.commentsList = {
          ...incomingData,
          items: [...existingItems, ...newItems],
        };
        this.hasLoaded.set(true);
        if (isFirstLoad) {
          this.commentsLoaded.emit();
        }
      },
      error: () => {
        this.isLoadingMore = false;
      },
    });
  }

  prependSignalRComment(newComment: CommentDto) {
    this.commentsList = {
      ...this.commentsList,
      totalItems: (this.commentsList.totalItems || 0) + 1,
      items: [newComment, ...this.commentsList.items],
    };
    this.cd.detectChanges();
  }

  onScrollIndexChange(index: number) {
    const currentItemsCount = this.commentsList?.items?.length || 0;
    const totalPossibleItems = this.commentsList?.totalItems || 0;

    if (this.isLoadingMore || currentItemsCount >= totalPossibleItems) {
      return;
    }

    if (index >= currentItemsCount - 3) {
      this.request.paging.page++;
      this.loadComments();
    }
  }

  addNewComment() {
    if (this.currentUser.getDefaultRoute() == '/login') {
      this.router.navigate(['login']);
    } else if (this.comment().trim() != '') {
      const newComment: CreateCommentCommand = {
        userID: this.currentUser.userId() as number,
        postID: +this.postId,
        content: this.comment(),
      };
      this.commentsService.postComment(newComment).subscribe(() => {
        this.comment.set('');
        this.cd.detectChanges();
      });
    }
  }

  trackComment(index: number, item: CommentDto): any {
    return item.commentID || index;
  }

  private createEmptyPageResult<T>(): PageResult<T> {
    return {
      items: [],
      pageSize: 10,
      currentPage: 1,
      includedTotal: true,
      totalItems: 0,
      totalPages: 0,
    };
  }
  openReportCommentDialog(commentID: number): void {
    this.dialog.open(ReportCommentComponent, {
      data: { commentId: commentID },
    });
  }
  routeToProfile(id: number): void {
    this.router.navigate(['profile'], {
      queryParams: {
        userID: id,
      },
    });
  }
}
