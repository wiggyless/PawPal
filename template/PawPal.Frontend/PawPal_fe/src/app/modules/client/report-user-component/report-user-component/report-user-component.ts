import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { REPORT_REASON_USER_LABELS } from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.model';
import { ReportUserService } from '../../../../api-services/moderation/reported-posts/reported-users/reported-users.service';

export interface ReportUserDialogData {
  reportedUserID: number;
  reportSentByID: number;
}

@Component({
  selector: 'app-report-user-dialog',
  standalone: false,
  templateUrl: './report-user-component.html',
  styleUrls: ['./report-user-component.scss'],
})
export class ReportUserComponent implements OnInit {
  form!: FormGroup;
  reasons = REPORT_REASON_USER_LABELS;
  isSubmitting = false;
  submitted = false;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private service: ReportUserService,
    private dialogRef: MatDialogRef<ReportUserComponent>,
    private cd: ChangeDetectorRef,
    @Inject(MAT_DIALOG_DATA) public data: ReportUserDialogData,
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
      .createUserReport({
        reason: this.form.value.reason,
        description: this.form.value.description?.trim() || undefined,
        reportedUserID: this.data.reportedUserID as number,
        reportCreatedByUserID: this.data.reportSentByID,
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
