import { Component, inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormBuilder,
  FormControl,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { UserService } from '../../../../api-services/users/users-service';
import { CreateUserCommand } from '../../../../api-services/users/users-model';
import { Router } from '@angular/router';
import { catchError, debounceTime, distinctUntilChanged, first, map, Observable, of, switchMap } from 'rxjs';
import { trigger, transition, style, animate } from '@angular/animations';

export const passwordMatchValidator: ValidatorFn = (
  group: AbstractControl,
): ValidationErrors | null => {
  const password = group.get('password')?.value;
  const repeatPassword = group.get('repeatPassword')?.value;
  return password && repeatPassword && password !== repeatPassword
    ? { passwordMismatch: true }
    : null;
};

export function usernameTakenValidator(userService: UserService): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    const value = control.value?.toString().trim();
    if (!value) return of(null);

    return of(value).pipe(
      debounceTime(600),
      distinctUntilChanged(),
      switchMap((username) => userService.getByUsername(username)),
      map((res) => (res?.exists ? { usernameTaken: true } : null)),
      catchError(() => of(null)),
      first(),
    );
  };
}
export function emailTakenValidator(userService: UserService): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    const value = control.value?.toString().trim();
    if (!value) return of(null);

    return of(value).pipe(
      debounceTime(600),
      distinctUntilChanged(),
      switchMap((email) => userService.getByEmail(email)),
      map((res) => (res?.exists ? { emailTaken: true } : null)),
      catchError(() => of(null)),
      first(),
    );
  };
}

@Component({
  selector: 'app-register-component',
  standalone: false,
  templateUrl: './register-component.html',
  styleUrl: './register-component.scss',
  animations: [
    trigger('fadeSlide', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-4px)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
      transition(':leave', [
        animate('150ms ease-in', style({ opacity: 0, transform: 'translateY(-4px)' })),
      ]),
    ]),
  ],
})

export class RegisterComponent implements OnInit {
  private _formBuilder = inject(FormBuilder);
  private cityService = inject(CitiesService);
  private userService = inject(UserService);
  private router = inject(Router);

  cityList: any = [];
  cityId: number = 0;
  dateOfBirth: Date = new Date();
  showPassword = false;
  showRepeatPassword = false;
  dateControl = new FormControl(new Date());

  basicInfo = this._formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    cityId: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
  });

  accountInfo = this._formBuilder.group(
    {
      username: [
        '',
        [Validators.required],
        [usernameTakenValidator(this.userService)],
      ],
      email: [
        '',
        [Validators.required, Validators.email],
        [emailTakenValidator(this.userService)],
      ],
      password: ['', [Validators.required, Validators.minLength(8)]],
      repeatPassword: ['', Validators.required],
    },
    { validators: passwordMatchValidator },
  );

  additionalInfo = this._formBuilder.group({
    aboutMe: ['', [Validators.maxLength(500), Validators.required]],
    favouriteAnimal: [''],
  });

  ngOnInit(): void {
    this.loadCities();
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

 get usernameControl(): AbstractControl | null {
  return this.accountInfo.get('username');
}

  get emailControl() {
    return this.accountInfo.get('email');
  }

  get usernamePending(): boolean {
    return this.usernameControl?.pending ?? false;
  }

  get emailPending(): boolean {
    return this.emailControl?.pending ?? false;
  }

  onSubmit() {
    if (
      this.accountInfo.invalid ||
      this.accountInfo.pending ||
      this.basicInfo.invalid ||
      this.additionalInfo.invalid
    ) {
      this.accountInfo.markAllAsTouched();
      this.basicInfo.markAllAsTouched();
      this.additionalInfo.markAllAsTouched();
      return;
    }

    const payload: CreateUserCommand = {
      firstName: this.basicInfo.value.firstName ?? '',
      lastName: this.basicInfo.value.lastName ?? '',
      birthDate: new Date(this.basicInfo.value.dateOfBirth ?? ''),
      username: this.accountInfo.value.username ?? '',
      email: this.accountInfo.value.email ?? '',
      password: this.accountInfo.value.password ?? '',
      city: this.basicInfo.value.cityId ?? 0,
      aboutMe: this.additionalInfo.value.aboutMe ?? '',
    };

    // A profile picture can be added later from Settings, once the account has a real
    // session — uploads no longer happen anonymously during signup.
    this.userService.createUser(payload).subscribe({
      next: () => this.navigateAfterRegister(),
      error: (err) => {
        console.error('Registration error:', err);
      },
    });
  }

  private navigateAfterRegister(): void {
    this.router.navigate(['/auth/login'], {
      queryParams: { message: 'Please check your e-mail to confirm your account.' },
    });
  }
}
