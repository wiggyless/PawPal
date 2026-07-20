import { Component, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../app/core/components/base-classes/base-component';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthFacadeService } from '../../../../app/core/services/auth/auth-facade.service';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../../app/core/services/auth/current-user.service';
import { LoginCommand } from '../../../../app/api-services/auth/auth-api.model';
import { DialoguePopupService } from '../../../../app/api-services/dialogue-popup/dialogue-popup.service';
import { ActivatedRoute } from '@angular/router';
import { AuthTimeoutService } from '../../../../app/core/services/auth/auth-timeout.service';
import { MatDialog } from '@angular/material/dialog';
import { PasswordRecoveryDialog } from './password-recovery-dialog/password-recovery-dialog/password-recovery-dialog';

declare const grecaptcha: any;

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class LoginComponent extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);
  private currentUser = inject(CurrentUserService);
  private dialogueService = inject(DialoguePopupService);
  private route = inject(ActivatedRoute);
  private authTimeoutService = inject(AuthTimeoutService);
  private dialog = inject(MatDialog);

  showPassword = false;
  showResend = false;
  resendSuccess = false;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  ngOnInit(): void {
    const message = this.route.snapshot.queryParamMap.get('message');
    if (message) {
      this.dialogueService.success('Registration Successful!', message);
    }

    setTimeout(() => {
      if (typeof grecaptcha !== 'undefined') {
        grecaptcha.render('recaptcha-container', {
          sitekey: '6Le7KPcsAAAAAPFwAFtqrrAaxMiQqNIRyxaAuyAu',
        });
      }
    }, 500);
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    if (typeof grecaptcha === 'undefined') {
      alert('CAPTCHA not loaded yet. Please wait a moment and try again.');
      return;
    }
    const token = grecaptcha.getResponse();

    if (!token) {
      this.dialogueService.error('CAPTCHA Required', 'Please complete the CAPTCHA to proceed.');
      return;
    }

    const payload: LoginCommand = {
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? '',
      fingerprint: null,
      recaptchaToken: token,
    };
    this.auth.login(payload).subscribe({
      next: () => {
        const target = this.currentUser.getDefaultRoute();
        this.authTimeoutService.startExpirationTracker();
        this.router.navigate(['/']);
        grecaptcha.reset();
      },
      error: (err) => {
        const message = err.error?.message || '';
        if (
          message.toLowerCase().includes('verify') ||
          message.toLowerCase().includes('confirmed')
        ) {
          this.showResend = true;
        }
        this.dialogueService.error(
          'Login Failed',
          err.error?.message || 'An error occurred during login. Please try again.',
        );
        console.error('Login error:', err);
        grecaptcha.reset();
      },
    });
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  loadPasswordRecoveryDialog(): void {
    this.dialog.open(PasswordRecoveryDialog, {
      width: '50%',
      panelClass: 'transparent-dialog-panel',
      hasBackdrop: true,
    });
  }
  resendEmail(): void {
    const email = this.form.value.email ?? '';
    this.auth.resendConfirmationEmail(email).subscribe({
      next: () => (this.resendSuccess = true),
      error: () =>
        this.dialogueService.error(
          'Error',
          'Could not resend confirmation email. Please try again.',
        ),
    });
  }
}
