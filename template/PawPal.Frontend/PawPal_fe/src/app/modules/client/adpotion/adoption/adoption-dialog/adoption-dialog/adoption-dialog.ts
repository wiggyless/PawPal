import { Location } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-adoption-dialog',
  standalone: false,
  templateUrl: './adoption-dialog.html',
  styleUrl: './adoption-dialog.scss'
})
export class AdoptionDialog {
  router = inject(Router);
  matDialog = inject(MatDialogRef);
  getMeBack() {
    this.matDialog.close();
    this.router.navigate(['']);
  }
}
