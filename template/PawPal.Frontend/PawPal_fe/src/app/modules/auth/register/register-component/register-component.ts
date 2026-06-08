import { Component, inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { UserService } from '../../../../api-services/users/users-service';
import { CreateUserCommand } from '../../../../api-services/users/users-model';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { LoginCommand } from '../../../../api-services/auth/auth-api.model';
import { Router } from '@angular/router';
import { catchError, debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
import { trigger, transition, style, animate } from '@angular/animations';

export const passwordMatchValidator: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
  const password = group.get('password')?.value;
  const repeatPassword = group.get('repeatPassword')?.value;
  return password && repeatPassword && password !== repeatPassword
    ? { passwordMismatch: true }
    : null;
};

@Component({
  selector: 'app-register-component',
  standalone: false,
  templateUrl: './register-component.html',
  styleUrl: './register-component.scss',
  animations: [ 
    trigger('fadeSlide', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-4px)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('150ms ease-in', style({ opacity: 0, transform: 'translateY(-4px)' }))
      ])
    ])
  ]
})
export class RegisterComponent implements OnInit {

  private _formBuilder = inject(FormBuilder);
  private cityService = inject(CitiesService);
  private userService = inject(UserService);
  private auth = inject(AuthFacadeService);
  private currentUser = inject(CurrentUserService);
  private router = inject(Router);

  cityList: any = [];
  cityId: number = 0;
  dateOfBirth: Date = new Date();
  showPassword = false;
  showRepeatPassword = false;
  dateControl = new FormControl(new Date());
  usernameAvailable = false;
  emailAvailable = false;

  basicInfo = this._formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    cityId: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
  });

  accountInfo = this._formBuilder.group(
    {
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      repeatPassword: ['', Validators.required],
    },
    { validators: passwordMatchValidator }
  );

  additionalInfo = this._formBuilder.group({
    aboutMe: ['', [Validators.maxLength(500), Validators.required]],
    favouriteAnimal: [''],
  });
  ngOnInit(): void {
    this.loadCities();
    this.accountInfo.get('username')?.valueChanges.pipe( 
      catchError(() => of(null)),
      debounceTime(600),
      distinctUntilChanged(),
      switchMap(username => this.userService.getByUsername(username?.toString() || ''))
    ).subscribe({
      next: (res) => {
        if (res != null) {
          this.accountInfo.get('username')?.setErrors({ usernameTaken: true });
          this.usernameAvailable = false;
        }
        else {
          this.accountInfo.get('username')?.setErrors(null);
          this.usernameAvailable = true;
        }
      },
      error: (err) => {
        console.error('Error checking username availability:', err);
      }
    });
    if(this.accountInfo.get('email') == null) return;
    this.accountInfo.get('email')?.valueChanges.pipe(
      catchError(() => of(null)),
      debounceTime(600),
      distinctUntilChanged(),
      switchMap(email => this.userService.getByEmail(email?.toString() || ''))
    ).subscribe({
      next: (res) => {
        if (res != null) {
          this.accountInfo.get('email')?.setErrors({ emailTaken: true });
          this.emailAvailable = false;
        }
        else {
          this.accountInfo.get('email')?.setErrors(null);
          this.emailAvailable = true;
        }
      }
      , error: (err) => {
        console.error('Error checking email availability:', err);
      }
    });

  }
  loadCities(): void {
    this.cityService.listCities().subscribe((res) => {
      this.cityList = res;
    });
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  toggleRepeatPassword(): void {
    this.showRepeatPassword = !this.showRepeatPassword;
  }

  onSubmit() {
    if (this.accountInfo.invalid || this.basicInfo.invalid || this.additionalInfo.invalid) return;

    const payload: CreateUserCommand = {
      firstName: this.basicInfo.value.firstName ?? '',
      lastName: this.basicInfo.value.lastName ?? '',
      birthDate: new Date(this.basicInfo.value.dateOfBirth ?? ''),
      username: this.accountInfo.value.username ?? '',
      email: this.accountInfo.value.email ?? '',
      password: this.accountInfo.value.password ?? '',
      roleID: 2,
      city: this.basicInfo.value.cityId ?? 0,
      profilePictureURL: null,
    };

    this.userService.createUser(payload).subscribe({
      next: () => {
         this.router.navigate(['/auth/login'], {
      queryParams: { message: 'Please check your e-mail to confirm your account.' }
    });
      },
      error: (err) => { console.error('Registration error:', err); },
    });
  }
}
