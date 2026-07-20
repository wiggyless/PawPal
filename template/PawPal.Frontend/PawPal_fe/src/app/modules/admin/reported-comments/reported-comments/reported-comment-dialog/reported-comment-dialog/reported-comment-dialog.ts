import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CommentService } from '../../../../../../api-services/comments/comments.service';
import { environment } from '../../../../../../../environments/environment';
import { UserImageService } from '../../../../../../api-services/userImage/userImage-service';
import { UserService } from '../../../../../../api-services/users/users-service';
import { Router } from '@angular/router';
import { REPORT_REASON_COMMENTS_LABELS } from '../../../../../../api-services/moderation/reported-posts/reported-comments/reported-comments.model';
import { take } from 'rxjs';
import { DialoguePopupService } from '../../../../../../api-services/dialogue-popup/dialogue-popup.service';

@Component({
  selector: 'app-reported-comment-dialog',
  standalone: false,
  templateUrl: './reported-comment-dialog.html',
  styleUrl: './reported-comment-dialog.scss',
})
export class ReportedCommentDialog implements OnInit {
  dialogRef = inject(MatDialogRef);
  dialogData = inject(MAT_DIALOG_DATA);
  commentService = inject(CommentService);
  dialogPopUp = inject(DialoguePopupService);
  router = inject(Router);
  env = environment.apiUrl;
  reasons = REPORT_REASON_COMMENTS_LABELS;
  ngOnInit(): void {}
  closeDialog() {
    this.dialogRef.close(false);
  }
  routeToPost(postID: number) {
    this.dialogRef.close();
    this.router.navigate(['post'], {
      queryParams: {
        postID: postID,
      },
    });
  }
  deleteComment() {
    this.dialogPopUp.warning(
      'Confirm action',
      'Are you sure that you want to delete this comment?',
      'Yes',
      'No',
      () => {
        this.commentService
          .deleteComment(this.dialogData.comment.id)
          .pipe(take(1))
          .subscribe({
            next: (res) => {
              this.dialogRef.close(true);
              this.dialogPopUp.success('Success', 'Comment has been successfully deleted!', 'OK');
            },
            error: (res) => {
              this.dialogRef.close(false);
              this.dialogPopUp.error('Error', 'An error has occurred, please try again', 'OK');
            },
          });
      },
      () => {},
    );
  }
}
