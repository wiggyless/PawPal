import { DialogRef } from '@angular/cdk/dialog';
import { ChangeDetectorRef, Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import {
  GetAndPostAnswerDTO,
  IsAnswerTrue,
} from '../../../../../../api-services/security/answers/answer-model';
import { SecurityAnswerService } from '../../../../../../api-services/security/answers/answer-service';
import {
  GetSecurityQuestionDTO,
  GetSecurityQuestionsQueryByEmail,
} from '../../../../../../api-services/security/questions/questions-model';
import { SecurityQuestionService } from '../../../../../../api-services/security/questions/questions-service';
import { CurrentUserService } from '../../../../../../core/services/auth/current-user.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Subject, take, takeUntil } from 'rxjs';
import { UserService } from '../../../../../../api-services/users/users-service';
import { MatStepper } from '@angular/material/stepper';
@Component({
  selector: 'app-password-recovery-dialog',
  standalone: false,
  templateUrl: './password-recovery-dialog.html',
  styleUrl: './password-recovery-dialog.scss',
})
export class PasswordRecoveryDialog {
  securityQuestions = inject(SecurityQuestionService);
  currentUserService = inject(CurrentUserService);
  answerService = inject(SecurityAnswerService);
  userService = inject(UserService);
  private _formBuilder = inject(FormBuilder);
  cd = inject(ChangeDetectorRef);
  emailFormGroup = this._formBuilder.group({
    email: ['', Validators.required],
  });

  securityFormGroup = this._formBuilder.group({
    firstQuestion: [0, Validators.required],
    firstAnswer: ['', Validators.required],
    secondQuestion: [0, Validators.required],
    secondAnswer: ['', Validators.required],
    thirdQuestion: [0, Validators.required],
    thirdAnswer: ['', Validators.required],
  });
  passwordFormGroup = this._formBuilder.group({
    password: ['', [Validators.required, Validators.minLength(8)]],
    secondPassword: ['', Validators.required],
  });
  change = signal(false);
  dialogRef = inject(DialogRef);
  loading = signal(false);
  error = signal(false);
  lastEmail = '';
  answerCorrect: IsAnswerTrue | undefined;
  securityQuestionList = signal<GetSecurityQuestionDTO[]>([]);
  showPassword = false;
  private destroyRef = new Subject<void>();
  ngOnInit(): void {}
  ngOnDestroy(): void {
    this.destroyRef.next();
    this.destroyRef.complete();
  }
  checkEmail(): void {
    const email = this.emailFormGroup.get('email')?.value as string;
    if (!email || this.lastEmail === email) return;

    this.loading.set(true);
    this.error.set(false);
    this.lastEmail = email;

    this.securityQuestions
      .getSecurityQuestionsByEmail({ email: email, paging: { page: 1, pageSize: 10 } })
      .pipe(takeUntil(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.securityQuestionList.set(response.items);
          this.loading.set(false);
          this.change.set(true);
        },
        error: () => {
          this.error.set(true);
          this.loading.set(false);
        },
      });
  }
  Question(count: string) {
    return this.securityFormGroup.get(count + 'Question')!.value as number;
  }
  doesItExist(id: number, count: string): boolean {
    return id == this.Question(count);
  }
  Answer(count: string) {
    return this.securityFormGroup.get(count + 'Answer')!.value;
  }
  getMeBack() {
    this.dialogRef.close();
  }
  checkAnswers(stepper: MatStepper) {
    const answersRecord: Record<number, string> = {
      [this.Question('first')]: this.Answer('first') as string,
      [this.Question('second')]: this.Answer('second') as string,
      [this.Question('third')]: this.Answer('third') as string,
    };
    this.answerService
      .checkSecurityAnswer({
        answers: answersRecord,
        email: this.emailFormGroup.get('email')!.value as string,
      })
      .pipe(take(1))
      .subscribe({
        next: (response) => {
          if (response.isTrueAnswer) {
            this.answerCorrect = response;
            stepper.selected!.completed = true;
            console.log(response.isTrueAnswer);
            this.cd.detectChanges();
            stepper.next();
          }
        },
        error: (response) => {
          console.log('WRONG ANSWER');
        },
      });
  }
  updatePassword() {
    if (this.currentUserService.email() != null && this.passwordFormGroup.valid) {
      this.userService
        .updatePassword({
          email: this.emailFormGroup.get('email')!.value as string,
          newPassword: this.passwordFormGroup.get('password')!.value as string,
        })
        .pipe(take(1))
        .subscribe({
          next: (res) => {
            console.log('Password saved');
            this.dialogRef.close();
          },
          error: (res) => {
            console.log('THRE WAS AN ERROR');
          },
        });
    }
  }
  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
}
