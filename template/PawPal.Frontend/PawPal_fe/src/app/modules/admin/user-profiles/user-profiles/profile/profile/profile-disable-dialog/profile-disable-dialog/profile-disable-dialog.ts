import { ChangeDetectorRef, Component, inject, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { REPORT_REASON_USER_LABELS } from '../../../../../../../../api-services/moderation/reported-posts/reported-users/reported-users.model';
import { CurrentUserService } from '../../../../../../../../core/services/auth/current-user.service';
import { UserDisabledService } from '../../../../../../../../api-services/users-disabled/users-disabled.service';
import { DialoguePopupService } from '../../../../../../../../api-services/dialogue-popup/dialogue-popup.service';

export interface ReportCommentDialog {
  userId: number;
}

@Component({
  selector: 'app-profile-disable-dialog',
  standalone: false,
  templateUrl: './profile-disable-dialog.html',
  styleUrls: ['./profile-disable-dialog.scss'],
})
export class ProfileDisableDialog implements OnInit {
  form!: FormGroup;
  reasons = REPORT_REASON_USER_LABELS;
  isSubmitting = false;
  submitted = false;
  errorMessage: string | null = null;
  currentUser = inject(CurrentUserService);
  dialogPopUp = inject(DialoguePopupService);
  route = inject(Router);
  constructor(
    private fb: FormBuilder,
    private service: UserDisabledService,
    private dialogRef: MatDialogRef<ProfileDisableDialog>,
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
      .createUserDisable({
        reason: this.reasons[this.form.value.reason].label,
        description: this.form.value.description?.trim() || undefined,
        userID: this.data.userId,
        dateDisabled: new Date(),
      })
      .subscribe({
        next: (res) => {
          this.isSubmitting = false;
          this.submitted = true;
          //this.dialogPopUp.info('Success', 'User profile has been successfuly disabled', 'OK');
          this.cd.detectChanges();
        },
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = 'Something went wrong. Please try again.';
          //this.dialogPopUp.info('Error', this.errorMessage, 'OK');
        },
      });
  }

  onDone(): void {
    this.dialogRef.close(true);
  }
}
