import { Component, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-classes/base-component';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { LoginCommand } from '../../../../api-services/auth/auth-api.model';
import { DialoguePopupService } from '../../../shared/components/dialogue-popup/dialogue-popup.service';
import { ActivatedRoute } from '@angular/router'; 
import { AuthTimeoutService } from '../../../../core/services/auth/auth-timeout.service';

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
  private route = inject(ActivatedRoute)
  private authTimeoutService = inject(AuthTimeoutService);
  showPassword = false;

  ngOnInit(): void {
  const message = this.route.snapshot.queryParamMap.get('message');
  if (message) {
    this.dialogueService.success('Registration Successful!', message);
  }

  // always render captcha, not just when there's a message
  setTimeout(() => {
    if (typeof grecaptcha !== 'undefined') {
      grecaptcha.render('recaptcha-container', {
        sitekey: '6Le7KPcsAAAAAPFwAFtqrrAaxMiQqNIRyxaAuyAu'
      });
    }
  }, 500);
}
 
  
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

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
    console.log(payload);
    this.auth.login(payload).subscribe({
      next: () => {
        const target = this.currentUser.getDefaultRoute();
        this.authTimeoutService.startExpirationTracker();
        this.router.navigate([target]);
        grecaptcha.reset();
      },
      error: (err) => {
        this.dialogueService.error('Login Failed', err.error?.message || 'An error occurred during login. Please try again.');
        console.error('Login error:', err);
        grecaptcha.reset();
      },
    });
    
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
}
