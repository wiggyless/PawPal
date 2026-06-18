import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { UserService } from '../../../api-services/users/users-service';
import { DialoguePopupService } from '../../../api-services/dialogue-popup/dialogue-popup.service';
import { SecurityQuestionsDialog } from './securityQuestions-dialog/security-questions-dialog/security-questions-dialog';
import { DyanmicThemeService } from '../../../core/services/dynamic-theme.service';
import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings-component.html',
  styleUrl: './settings-component.scss',
})
export class SettingsComponent implements OnInit {
  currentUser = inject(CurrentUserService);
  userService = inject(UserService);
  cd = inject(ChangeDetectorRef);
  darkMode = inject(DyanmicThemeService);
  dialog = inject(MatDialog);
  dialogPopUp = inject(DialoguePopupService);
  
  userEmail: string = '';
  showChangeEmailForm: boolean = false;
  showChangePasswordForm: boolean = false;
  newEmail: string = '';
  currentPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  isDarkTheme: boolean = false;
  


  ngOnInit(): void {
    this.userService.getUser(this.currentUser.userId() as number).subscribe((res) => {
      this.userEmail = res.email;
      this.cd.detectChanges();
    });

    this.isDarkTheme = localStorage.getItem('theme') === 'dark';
   document.body.classList.toggle('dark-theme', this.isDarkTheme);

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

    this.showChangePasswordForm = false;
    this.currentPassword = '';
    this.newPassword = '';
    this.confirmPassword = '';
    alert('Password updated successfully');
  }

  openDeleteProfileDialog(): void {
    this.dialogPopUp.warning('Delete your profile?', 'This action is permanent. Once you delete your account, there is no going back. Please be certain.', 'DELETE', 'CANCEL');
  }
  enableSecurityQuestions() {
    this.dialog.open(SecurityQuestionsDialog);
  }

    toggleTheme(): void {
    this.darkMode.toggleTheme();
    document.body.classList.toggle('dark-theme', this.isDarkTheme);
  }


}
