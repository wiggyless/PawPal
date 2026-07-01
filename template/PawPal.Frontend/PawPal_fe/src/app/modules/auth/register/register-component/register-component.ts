import { Component, inject, OnInit, signal } from '@angular/core';
import {
  AbstractControl,
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
import { catchError, debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';
import { trigger, transition, style, animate } from '@angular/animations';
import { UserImageService } from '../../../../api-services/userImage/userImage-service';
import {
  CropDialogResult,
  UserProfileImageCropDialog,
} from '../../../client/my-profile/user-profile-component/user-profile-imageCrop/user-profile-image-crop-dialog/user-profile-image-crop-dialog';
import { DialogRef } from '@angular/cdk/dialog';
import { MatDialog } from '@angular/material/dialog';
import { SafeUrl } from '@angular/platform-browser';

export const passwordMatchValidator: ValidatorFn = (
  group: AbstractControl,
): ValidationErrors | null => {
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
  private userImageService = inject(UserImageService);
  cityList: any = [];
  cityId: number = 0;
  dateOfBirth: Date = new Date();
  showPassword = false;
  showRepeatPassword = false;
  dateControl = new FormControl(new Date());
  usernameAvailable = false;
  emailAvailable = false;
  dialogRef = inject(MatDialog);
  basicInfo = this._formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    cityId: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
  });
  imageChanged = signal(false);
  accountInfo = this._formBuilder.group(
    {
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      repeatPassword: ['', Validators.required],
    },
    { validators: passwordMatchValidator },
  );
  profileImagePreview: string | null = null;
  selectedProfileImage: File | undefined;
  imageUrl = signal<SafeUrl | null>(null);
  /*
onProfileImageSelected(event: any) {
  const file = event.target.files?.[0];
  if (file) {
    this.selectedProfileImage = file;
    const reader = new FileReader();
    reader.onload = () => {
      this.profileImagePreview = reader.result as string;
    };
    reader.readAsDataURL(file);
  }
}
  */
  onProfileImageSelected(event: any): void {
    const files = event.target.files;
    if (!files || files.length === 0) return;

    const file: File = files[0];

    event.target.value = '';
    this.dialogRef
      .open(UserProfileImageCropDialog, {
        data: { imageFile: file },
        width: '40vw',
        maxWidth: '40vw',
        maxHeight: '95vh',
        disableClose: true,
        panelClass: 'image-crop-dialog-panel',
      })
      .afterClosed()
      .subscribe((result: CropDialogResult | undefined) => {
        if (!result) return;

        this.selectedProfileImage = result.croppedFile;
        this.imageChanged.set(true);
        this.imageUrl.set(result.croppedUrl);
      });
  }
  additionalInfo = this._formBuilder.group({
    aboutMe: ['', [Validators.maxLength(500), Validators.required]],
    favouriteAnimal: [''],
  });
  ngOnInit(): void {
    this.loadCities();
    this.accountInfo
      .get('username')
      ?.valueChanges.pipe(
        catchError(() => of(null)),
        debounceTime(600),
        distinctUntilChanged(),
        switchMap((username) => this.userService.getByUsername(username?.toString() || '')),
      )
      .subscribe({
        next: (res) => {
          if (res != null) {
            this.accountInfo.get('username')?.setErrors({ usernameTaken: true });
            this.usernameAvailable = false;
          } else {
            this.accountInfo.get('username')?.setErrors(null);
            this.usernameAvailable = true;
          }
        },
        error: (err) => {
          console.error('Error checking username availability:', err);
        },
      });
    if (this.accountInfo.get('email') == null) return;
    this.accountInfo
      .get('email')
      ?.valueChanges.pipe(
        catchError(() => of(null)),
        debounceTime(600),
        distinctUntilChanged(),
        switchMap((email) => this.userService.getByEmail(email?.toString() || '')),
      )
      .subscribe({
        next: (res) => {
          if (res != null) {
            this.accountInfo.get('email')?.setErrors({ emailTaken: true });
            this.emailAvailable = false;
          } else {
            this.accountInfo.get('email')?.setErrors(null);
            this.emailAvailable = true;
          }
        },
        error: (err) => {
          console.error('Error checking email availability:', err);
        },
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
      aboutMe: this.additionalInfo.value.aboutMe ?? '',
    };

    this.userService.createUser(payload).subscribe({
      next: (response: { id: number }) => {
        if (this.selectedProfileImage && response.id) {
          this.userImageService.createUserImage(response.id, this.selectedProfileImage).subscribe({
            next: () => this.navigateAfterRegister(),
            error: (err) => {
              console.error('Image upload failed:', err);
              this.navigateAfterRegister();
            },
          });
        } else {
          this.navigateAfterRegister();
        }
      },
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
