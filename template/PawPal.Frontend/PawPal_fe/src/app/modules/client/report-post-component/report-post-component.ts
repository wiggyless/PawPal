import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReportedPostsService } from '../../../api-services/moderation/reported-posts/reported-posts.service';
import { REPORT_REASON_LABELS } from '../../../api-services/moderation/reported-posts/reported-posts.model';

export interface ReportPostDialogData {
  postId: number;
  userId: number;
}

@Component({
  selector: 'app-report-post-dialog',
  standalone: false,
  templateUrl: './report-post-component.html',
  styleUrls: ['./report-post-component.scss'],
})
export class ReportPostComponent implements OnInit {
  form!: FormGroup;
  reasons = REPORT_REASON_LABELS;
  isSubmitting = false;
  submitted = false;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private service: ReportedPostsService,
    private dialogRef: MatDialogRef<ReportPostComponent>,
    private cd: ChangeDetectorRef,
    @Inject(MAT_DIALOG_DATA) public data: ReportPostDialogData,
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

    this.isSubmitting = true;
    this.errorMessage = null;

    this.service
      .createReport({
        reason: this.form.value.reason,
        description: this.form.value.description?.trim() || undefined,
        postID: this.data.postId,
        userID: this.data.userId,
        dateSent: new Date(),
      })
      .subscribe({
        next: (res) => {
          this.isSubmitting = false;
          this.submitted = true;
          this.cd.detectChanges();
        },
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = 'Something went wrong. Please try again.';
        },
      });
  }

  onDone(): void {
    this.dialogRef.close(true);
  }
}
