import { Component, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-post-dialog',
  standalone: false,
  templateUrl: './create-post-dialog.html',
  styleUrl: './create-post-dialog.scss',
})
export class CreatePostDialog {
  router = inject(Router);
  dialogRef = inject(MatDialogRef);
  getMeBack() {
    this.dialogRef.close();
  }
}
