import { ChangeDetectorRef, Component, inject, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { REPORT_REASON_COMMENTS_LABELS } from '../../../../api-services/moderation/reported-posts/reported-comments/reported-comments.model';
import { ReportCommentService } from '../../../../api-services/moderation/reported-posts/reported-comments/reported-comments.service';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { Router } from '@angular/router';

export interface ReportCommentDialog {
  commentId: number;
}

@Component({
  selector: 'app-report-comment-dialog',
  standalone: false,
  templateUrl: './report-comment-component.html',
  styleUrls: ['./report-comment-component.scss'],
})
export class ReportCommentComponent implements OnInit {
  form!: FormGroup;
  reasons = REPORT_REASON_COMMENTS_LABELS;
  isSubmitting = false;
  submitted = false;
  errorMessage: string | null = null;
  currentUser = inject(CurrentUserService);
  route = inject(Router);
  constructor(
    private fb: FormBuilder,
    private service: ReportCommentService,
    private dialogRef: MatDialogRef<ReportCommentComponent>,
    private cd: ChangeDetectorRef,
    @Inject(MAT_DIALOG_DATA) public data: ReportCommentDialog,
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      reason: [null, Validators.required],
      description: [''],
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onSubmit(): void {
    if (this.form.invalid || this.isSubmitting) return;
    if (!this.currentUser.isAuthenticated()) {
      this.route.navigate(['login']);
    }
    if (this.currentUser.getDefaultRoute() == '/login') {
      this.route.navigate(['login']);
    }
    this.isSubmitting = true;
    this.errorMessage = null;

    this.service
      .createCommentReport({
        reason: this.form.value.reason,
        description: this.form.value.description?.trim() || undefined,
        commentID: this.data.commentId,
        commentReportedByID: this.currentUser.userId() as number,
        dateReported: new Date(),
      })
      .subscribe({
        next: (res) => {
          console.log('next fired', res);
          this.isSubmitting = false;
          this.submitted = true;
          this.cd.detectChanges();
        },
        error: (err) => {
          console.log('err fired', err);
          this.isSubmitting = false;
          this.errorMessage = 'Something went wrong. Please try again.';
        },
      });
  }

  onDone(): void {
    this.dialogRef.close(true);
  }
}
