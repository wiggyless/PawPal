import { Component, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-classes/base-component';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { LoginCommand } from '../../../../api-services/auth/auth-api.model';
import { DialoguePopupService } from '../../../shared/components/dialogue-popup/dialogue-popup.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class LoginComponent extends BaseComponent implements OnInit {
  ngOnInit(): void {
    
  }
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);
  private currentUser = inject(CurrentUserService);
  private dialogueService = inject(DialoguePopupService);

  showPassword = false;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  onSubmit(): void {
    if (this.form.invalid)
      return;
    
    const payload: LoginCommand = {
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? '',
      fingerprint: null,
    };
    console.log(payload);
    this.auth.login(payload).subscribe({
      next: () => {
        const target = this.currentUser.getDefaultRoute();
        this.router.navigate([target]);
      },
      error: (err) => {
        console.log('CALLED');
        this.dialogueService.error('Login Failed', 'Please check your credentials and try again.');
        console.error('Login error:', err);
      },
    });
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
}
