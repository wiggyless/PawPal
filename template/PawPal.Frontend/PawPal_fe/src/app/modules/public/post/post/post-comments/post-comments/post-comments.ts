import { ChangeDetectorRef, Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-post-comments',
  standalone: false,
  templateUrl: './post-comments.html',
  styleUrl: './post-comments.scss',
})
export class PostComments implements OnInit, OnDestroy {
  @Input({ required: true }) postId!: number;
  private signalRSubscription: Subscription = new Subscription();
  commentsService = inject(CommentService);
  commentsList: PageResult<CommentDto> | undefined;
  currentUser = inject(CurrentUserService);
  cd = inject(ChangeDetectorRef);
  hasLoaded: boolean = false;
  comment: string = '';
  constructor(private signalRService: SignalRService) {}
  ngOnInit(): void {
    console.log(this.postId);
    this.loadComments();
    this.signalRSubscription = this.signalRService.commentReceived$.subscribe((newComment) => {
      console.log(newComment.postID == this.postId);
      if (newComment && newComment.postID == this.postId) {
        // Add the new comment to the list instantly
        this.commentsList = {
          ...this.commentsList!,
          items: [newComment, ...this.commentsList!.items],
        };
        this.cd.detectChanges();
        console.log('WORKS comment');
      }
    });
  }
  ngOnDestroy(): void {
    this.signalRSubscription.unsubscribe();
  }
  loadComments() {
    const query: CommentQuery = {
      postID: this.postId,
    };
    this.commentsService.getComments(query).subscribe((comments) => {
      this.commentsList = comments;
      this.hasLoaded = true;
      this.cd.detectChanges();
    });
  }
  addNewComment() {
    if (this.comment.trim() != '') {
      const newComment: CreateCommentCommand = {
        userID: this.currentUser.userId as number,
        postID: +this.postId,
        content: this.comment,
      };
      this.commentsService.postComment(newComment).subscribe((response) => {
        this.comment = '';
        this.cd.detectChanges();
      });
    }
  }
  postComment() {}
}
