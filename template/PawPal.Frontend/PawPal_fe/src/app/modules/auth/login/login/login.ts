import { Component, inject } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-classes/base-component';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { LoginCommand } from '../../../../api-services/auth/auth-api.model';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class LoginComponent extends BaseComponent {
  private fb = inject(FormBuilder);
    private auth = inject(AuthFacadeService);
    private router = inject(Router);
    private currentUser = inject(CurrentUserService);
  hidePassword = true;

  form = this.fb.group({
      email: ['gugugaga', [Validators.required, Validators.email]],
    password: ['Admin123!', [Validators.required]],
    rememberMe: [false],
  });

  onSubmit(): void{
    if(this.form.invalid) //ako se ne unesu ispravni podaci, don't submit
      return;
      this.startLoading();
      //pravimo payload tipa logincommand, i popunjavamo ga vrijednostima iz forme
      const payload : LoginCommand={
        email : this.form.value.email ?? '',
        password : this.form.value.password ?? '',
        fingerprint : null
      };

      console.log(payload);
      this.auth.login(payload).subscribe({
      next: () => {
        this.stopLoading();
        console.log("Login successful");
        
        console.log(this.currentUser.role_id());
        const target = this.currentUser.getDefaultRoute();
        this.router.navigate([target]);
      },
      error: (err) => {
        this.stopLoading('Invalid credentials. Please try again.');
        console.error('Login error:', err);
      },
    });

  }

}
