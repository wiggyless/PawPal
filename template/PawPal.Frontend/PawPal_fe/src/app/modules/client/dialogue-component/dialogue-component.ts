import { Component, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { UserService } from '../../../api-services/users/users-service';
import { Router } from '@angular/router';
import { AuthApiService } from '../../../api-services/auth/auth-api.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';

@Component({
  selector: 'app-dialogue-component',
  standalone: false,
  templateUrl: './dialogue-component.html',
  styleUrl: './dialogue-component.scss',
})
export class DialogueComponent implements OnInit {
  userService = inject(CurrentUserService);
  userActualService = inject(UserService);
  auth = inject(AuthApiService);
  router = inject(Router);
  dialog = inject(MatDialog);
  postService = inject(AnimalPostService);

  postID: number = 0;
  animalID: number = 0;
  postDeletion: boolean = false;
  profileDeletion: boolean = false;

  dialogRef = inject(MatDialogRef<DialogueComponent>);
  data = inject<{
    postDelete: boolean;
    profileDelete: boolean;
    postId: number;
    animalId: number;
  }>(MAT_DIALOG_DATA);

  userId: any;
  ngOnInit(): void {
    this.userId = this.userService.userId;
    this.postDeletion = this.data.postDelete;
    this.profileDeletion = this.data.profileDelete;
    this.postID = this.data.postId;
    this.animalID = this.data.animalId;
  }
  onDeleteUser(): void {
    this.userActualService.deleteUser(this.userId).subscribe({
      next: (res) => {
        this.dialog.closeAll();
        this.router.navigate(['/auth/logout']);
      },
      error: (res) => {},
    });
  }
  onDeletePost(): void {
    this.postService.deletePost(this.postID, this.animalID).subscribe({
      next: (res) => {
        this.dialog.closeAll();
        if (this.userService.roleid() == 3) {
          this.router.navigate(['admin']);
          this.dialogRef.close(true);
        } else {
          this.router.navigate(['/client']);
          this.dialogRef.close(false);
        }
      },
      error: (res) => {},
    });
  }
}
