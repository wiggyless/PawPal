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
  newEmail: string = '';
  isSendingEmailChange: boolean = false;
  emailChangeRequested: boolean = false;
  pendingEmail: string = '';
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
    this.emailChangeRequested = false;
    this.pendingEmail = '';
    if (!this.showChangeEmailForm) {
      this.newEmail = '';
    }
  }

  saveEmail(): void {
    const newEmail = this.newEmail?.trim();
    if (!newEmail || newEmail === this.userEmail || this.isSendingEmailChange) {
      return;
    }

    this.isSendingEmailChange = true;
    this.userService
      .requestEmailChange(this.currentUser.userId() as number, { newEmail })
      .subscribe({
        next: () => {
          this.isSendingEmailChange = false;
          this.pendingEmail = newEmail;
          this.emailChangeRequested = true;
          this.newEmail = '';
        },
        error: (err) => {
          this.isSendingEmailChange = false;
          this.dialogPopUp.error(
            'Could not send verification email',
            err?.error?.message ?? 'Something went wrong. Please try again.',
          );
        },
      });
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
