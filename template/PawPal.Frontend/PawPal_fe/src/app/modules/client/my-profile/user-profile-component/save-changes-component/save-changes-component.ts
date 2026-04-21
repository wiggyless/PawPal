import { Component } from '@angular/core';
import { inject, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
@Component({
  selector: 'app-save-changes-component',
  standalone: false,
  templateUrl: './save-changes-component.html',
  styleUrl: './save-changes-component.scss',
})
export class SaveChangesComponent implements OnInit {
  router = inject(Router);
  matDialog = inject(MatDialogRef);
  getMeBack() {
    this.matDialog.close();
  }

  ngOnInit(): void {
    console.log('hi');
  }
}
