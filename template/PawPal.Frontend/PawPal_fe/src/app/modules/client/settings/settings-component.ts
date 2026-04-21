import { Component, OnInit, inject } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogueComponent } from '../dialogue-component/dialogue-component';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalUserService } from '../../../api-services/animal-users/animal-users-service';

@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings-component.html',
  styleUrl: './settings-component.scss'
})
export class SettingsComponent implements OnInit {
  dialog = inject(MatDialog);
  currentUser = inject(CurrentUserService);
  userService = inject(AnimalUserService);
  userEmail: string = '';
  showChangeEmailForm: boolean = false;
  showChangePasswordForm: boolean = false;
  newEmail: string = '';
  currentPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';

  ngOnInit(): void {
    this.userService.getUser(this.currentUser.userId).subscribe((res)=>{
      this.userEmail=res.email;
    })
  }

  toggleChangeEmailForm(): void {
    this.showChangeEmailForm = !this.showChangeEmailForm;
    if (!this.showChangeEmailForm) {
      this.newEmail = '';
    }
  }

  toggleChangePasswordForm(): void {
    this.showChangePasswordForm = !this.showChangePasswordForm;
    if (!this.showChangePasswordForm) {
      this.currentPassword = '';
      this.newPassword = '';
      this.confirmPassword = '';
    }
  }

  saveEmail(): void {
    if (this.newEmail && this.newEmail !== this.userEmail) {
      // Call service to update email
      // this.userService.updateEmail(this.newEmail).subscribe(...)
      this.userEmail = this.newEmail;
      this.showChangeEmailForm = false;
      this.newEmail = '';
    }
  }

  savePassword(): void {
    if (this.newPassword !== this.confirmPassword) {
      alert('Passwords do not match');
      return;
    }
    
    if (this.newPassword.length < 8) {
      alert('Password must be at least 8 characters');
      return;
    }

    // Call service to update password
    // this.userService.updatePassword(this.currentPassword, this.newPassword).subscribe(...)
    this.showChangePasswordForm = false;
    this.currentPassword = '';
    this.newPassword = '';
    this.confirmPassword = '';
    alert('Password updated successfully');
  }

  openDeleteProfileDialog(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = { postDelete: false, profileDelete: true };
    this.dialog.open(DialogueComponent, dialogConfig);
  }
}
