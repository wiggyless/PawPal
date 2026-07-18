import { ChangeDetectorRef, Component, OnInit, inject, signal } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { UserService } from '../../../api-services/users/users-service';
import { DialoguePopupService } from '../../../api-services/dialogue-popup/dialogue-popup.service';
import { SecurityQuestionsDialog } from './securityQuestions-dialog/security-questions-dialog/security-questions-dialog';
import { DyanmicThemeService } from '../../../core/services/dynamic-theme.service';
import { MatDialog } from '@angular/material/dialog';
import { SecurityQuestionService } from '../../../api-services/security/questions/questions-service';
import { FormBuilder, Validators } from '@angular/forms';
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
  securityQuestions = inject(SecurityQuestionService);
  userEmail: string = '';
  showChangeEmailForm: boolean = false;
  showChangePasswordForm = signal(false);
  newEmail: string = '';
  isSendingEmailChange: boolean = false;
  emailChangeRequested: boolean = false;
  pendingEmail: string = '';
  isDarkTheme: boolean = false;
  isSecurityEnabled = signal(false);
  showPassword = signal(false);
  private _formBuilder = inject(FormBuilder);
  passwordFormGroup = this._formBuilder.group({
    currentPassword: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required],
  });
  ngOnInit(): void {
    this.userService.getUser(this.currentUser.userId() as number).subscribe((res) => {
      this.userEmail = res.email;
      this.cd.detectChanges();
      this.securityQuestions
        .getSecurityQuestionsByEmail({ email: this.userEmail, paging: { page: 1, pageSize: 10 } })
        .subscribe({
          next: (res) => {
            this.isSecurityEnabled.set(true);
          },
          error: (res) => {
            this.isSecurityEnabled.set(false);
          },
        });
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

  toggleChangePasswordForm(): void {
    this.showChangePasswordForm.set(!this.showChangePasswordForm);
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
    if (
      this.passwordFormGroup.get('password')?.value !=
      this.passwordFormGroup.get('confirmPassword')?.value
    ) {
      this.dialogPopUp.error('Error', 'Passwords do not match', 'OK');
    } else if (this.passwordFormGroup.valid) {
      console.log(this.passwordFormGroup.get('currentPassword')?.value as string);
      this.userService
        .updatePassword({
          email: this.currentUser.email() as string,
          passwordRecovery: false,
          newPassword: this.passwordFormGroup.get('password')?.value as string,
          currentPassword: this.passwordFormGroup.get('currentPassword')?.value as string,
        })
        .subscribe({
          next: (res) => {
            this.dialogPopUp.success('Success', 'New password successfully saved', 'OK');
            this.showChangePasswordForm.set(!this.showChangePasswordForm);
          },
          error: (res) => {
            this.dialogPopUp.success('Errorr', res?.error.message, 'OK');
          },
        });
    } else {
      Object.keys(this.passwordFormGroup.controls).forEach((key) => {
        const controlErrors = this.passwordFormGroup.get(key)?.errors;

        if (controlErrors != null) {
          Object.keys(controlErrors).forEach((keyError) => {
            if (keyError === 'required') {
              this.dialogPopUp.error('Error', `Field [${key}] is empty.`, 'OK');
            } else if (keyError === 'minlength') {
              const actualLength = controlErrors[keyError].actualLength;
              const requiredLength = controlErrors[keyError].requiredLength;
              this.dialogPopUp.error(
                'Error',
                `Field [${key}] is too short. Current length: ${actualLength}, Required: ${requiredLength}`,
              );
            } else {
              this.dialogPopUp.error(
                'Error',
                `Field [${key}] has an error: ${keyError}`,
                controlErrors[keyError],
              );
            }
          });
        }
      });
    }
  }

  openDeleteProfileDialog(): void {
    this.dialogPopUp.warning(
      'Delete your profile?',
      'This action is permanent. Once you delete your account, there is no going back. Please be certain.',
      'DELETE',
      'CANCEL',
    );
  }
  enableSecurityQuestions() {
    this.dialog
      .open(SecurityQuestionsDialog, {
        data: {
          isEnabled: this.isSecurityEnabled(),
          email: this.userEmail,
        },
      })
      .afterClosed()
      .subscribe((res) => {
        this.securityQuestions
          .getSecurityQuestionsByEmail({ email: this.userEmail, paging: { page: 1, pageSize: 10 } })
          .subscribe({
            next: (res) => {
              this.isSecurityEnabled.set(true);
            },
            error: (res) => {
              this.isSecurityEnabled.set(false);
            },
          });
      });
  }

  toggleTheme(): void {
    this.darkMode.toggleTheme();
    document.body.classList.toggle('dark-theme', this.isDarkTheme);
  }
  togglePassword(): void {
    this.showPassword.set(!this.showPassword());
  }
}
