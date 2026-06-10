import { Component, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-session-warning-dialog',
  standalone: false,
  templateUrl: './rate-limit-dialog.html',
  styleUrl: './rate-limit-dialog.scss',
})
export class RateLimitDialog {
  dialogRef = inject(MatDialogRef);
  cancelSession() {
    this.dialogRef.close(false);
  }
  saveSession() {
    this.dialogRef.close(true);
  }
}
