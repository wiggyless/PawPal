import { Component, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-session-warning-dialog',
  standalone: false,
  templateUrl: './session-warning-dialog.html',
  styleUrl: './session-warning-dialog.scss',
})
export class SessionWarningDialog {
  dialogRef = inject(MatDialogRef);
  cancelSession() {
    this.dialogRef.close(false);
  }
  saveSession() {
    this.dialogRef.close(true);
  }
}
