import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  inject,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { SignalRService } from '../../../../../../core/services/signalr.service';
import { CommentService } from '../../../../../../api-services/comments/comments.service';
import {
  CommentDto,
  CreateCommentCommand,
  CommentQuery,
} from '../../../../../../api-services/comments/comments.model';
import { Observable, Subscription } from 'rxjs';
import { PageResult } from '../../../../../../core/models/paging/page-result';
import { CurrentUserService } from '../../../../../../core/services/auth/current-user.service';
import { PageEvent } from '@angular/material/paginator';
import { UserImageService } from '../../../../../../api-services/userImage/userImage-service';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-post-comments',
  standalone: false,
  templateUrl: './post-comments.html',
  styleUrl: './post-comments.scss',
})
export class PostComments implements OnInit, OnDestroy {
  @Input({ required: true }) postId!: number;
  @Output() commentsLoaded = new EventEmitter<void>();
  private signalRSubscription: Subscription = new Subscription();
  commentsService = inject(CommentService);
  commentsList: PageResult<CommentDto> | undefined;
  currentUser = inject(CurrentUserService);
  userImgService = inject(UserImageService);
  router = inject(Router);
  cd = inject(ChangeDetectorRef);
  hasLoaded: boolean = false;
  comment: string = '';
  page = {
    pageSize: 4,
    currentPage: 1,
    includedTotal: true,
    totalItems: 0,
    totalPages: 0,
    pageSizeOption: [4, 16, 32],
  };
  request: CommentQuery = {
    postID: 0,
    paging: {
      page: 1,
      pageSize: this.page.pageSize,
    },
  };
  counter: number = 0;
  objectUrl: string | null = null;
  private sanitizer = inject(DomSanitizer);

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.request.postID = this.postId;
    this.loadComments();
    this.signalRSubscription = this.signalRService.commentReceived$.subscribe((newComment) => {
      if (newComment && newComment.postID == this.postId) {
        this.commentsList = {
          ...this.commentsList!,
          items: [newComment, ...this.commentsList!.items],
        };
        this.cd.detectChanges();
      }
    });
  }
  ngOnDestroy(): void {
    this.signalRSubscription.unsubscribe();
  }
  loadComments() {
    this.commentsService.getComments(this.request).subscribe((comments) => {
      this.commentsList = comments;
      this.page = {
        pageSize: comments.pageSize == undefined ? 4 : comments.pageSize,
        currentPage: comments.currentPage,
        includedTotal: comments.includedTotal,
        totalItems: comments.totalItems,
        totalPages: comments.totalPages,
        pageSizeOption: this.page.pageSizeOption,
      };
      this.commentsList.items.forEach((element) => {
        this.userImgService.getUserImageByID(element.userID).subscribe({
          next: (safeUrl) => {
            element.imageData = safeUrl;
            this.counter++;
            this.checkCounter();
            this.commentsLoaded.emit();
          },
          error: () => {
            element.imageData = '';
            this.counter++;
            this.checkCounter();
            this.commentsLoaded.emit();
          },
        });
      });
    });
  }
  checkCounter() {
    if (this.counter == this.page.pageSizeOption[0]) {
      this.hasLoaded = true;
      this.counter = 0;
      this.cd.detectChanges();
    }
  }
  addNewComment() {
    if (this.currentUser.getDefaultRoute() == '/login') {
      this.router.navigate(['login']);
    } else if (this.comment.trim() != '') {
      const newComment: CreateCommentCommand = {
        userID: this.currentUser.userId() as number,
        postID: +this.postId,
        content: this.comment,
      };
      this.commentsService.postComment(newComment).subscribe((response) => {
        this.comment = '';
        this.cd.detectChanges();
      });
    }
  }
  handlePageEvent(event: PageEvent) {
    this.request.paging.page = event.pageIndex + 1;
    this.request.paging.pageSize = event.pageSize;
    this.loadComments();
  }
}
