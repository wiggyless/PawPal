import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { SecurityQuestionService } from '../../../../../api-services/security/questions/questions-service';
import { GetSecurityQuestionDTO } from '../../../../../api-services/security/questions/questions-model';
import { DialogRef } from '@angular/cdk/dialog';
import { GetAndPostAnswerDTO } from '../../../../../api-services/security/answers/answer-model';
import { CurrentUserService } from '../../../../../core/services/auth/current-user.service';
import { SecurityAnswerService } from '../../../../../api-services/security/answers/answer-service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialoguePopupService } from '../../../../../api-services/dialogue-popup/dialogue-popup.service';
import { forkJoin } from 'rxjs';
enum counts {
  first = 'first',
  second = 'second',
  third = 'third',
}
@Component({
  selector: 'app-security-questions-dialog',
  standalone: false,
  templateUrl: './security-questions-dialog.html',
  styleUrl: './security-questions-dialog.scss',
})
export class SecurityQuestionsDialog implements OnInit {
  securityQuestions = inject(SecurityQuestionService);
  securityQuestionList: Array<GetSecurityQuestionDTO> = [];
  currentUserService = inject(CurrentUserService);
  answerService = inject(SecurityAnswerService);
  matDialogData = inject(MAT_DIALOG_DATA);
  private _formBuilder = inject(FormBuilder);
  dialogPopUp = inject(DialoguePopupService);
  securityFormGroup = this._formBuilder.group({
    firstQuestion: [0, Validators.required],
    firstAnswer: ['', Validators.required],
    secondQuestion: [0, Validators.required],
    secondAnswer: ['', Validators.required],
    thirdQuestion: [0, Validators.required],
    thirdAnswer: ['', Validators.required],
  });
  dialogRef = inject(DialogRef);
  changes = inject(ChangeDetectorRef);
  ngOnInit(): void {
    if (this.matDialogData.isEnabled) {
      forkJoin({
        answeredQuestions: this.securityQuestions.getSecurityQuestionsByEmail({
          email: this.currentUserService.email() as string,
          paging: { page: 1, pageSize: 10 },
        }),
        questions: this.securityQuestions.getSecurityQuestions(),
      }).subscribe({
        next: (res) => {
          this.securityQuestionList = res.questions.items;
          this.securityFormGroup.patchValue({
            firstQuestion: res.answeredQuestions.items.at(0)?.id,
            secondQuestion: res.answeredQuestions.items.at(1)?.id,
            thirdQuestion: res.answeredQuestions.items.at(2)?.id,
          });
        },
        error: (res) => {},
      });
    } else {
      this.securityQuestions.getSecurityQuestions().subscribe((response) => {
        this.securityQuestionList = response.items;
      });
    }
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
  saveSecurityQuestions() {
    if (this.currentUserService.email() != null && this.securityFormGroup.valid) {
      const answersRecord: Record<number, string> = {
        [this.Question('first')]: this.Answer('first') as string,
        [this.Question('second')]: this.Answer('second') as string,
        [this.Question('third')]: this.Answer('third') as string,
      };
      const anwserDTO: GetAndPostAnswerDTO = {
        answers: answersRecord,
        email: this.currentUserService.email()!,
      };
      if (this.matDialogData.isEnabled) {
        this.answerService.updateSecurityAnswers(anwserDTO).subscribe({
          next: (res) => {
            this.dialogPopUp.error('Success', 'Your answers have been updated', 'OK');
            this.dialogRef.close();
          },
          error: (res) => {
            this.dialogPopUp.error('Error', res.error?.message, 'OK');
          },
        });
      } else {
        this.answerService.createSecurityAnswer(anwserDTO).subscribe({
          next: (res) => {
            this.dialogPopUp.error('Success', 'Your answers have been saved', 'OK');
            this.dialogRef.close();
          },
          error: (res) => {
            const rawMessage = res.error?.message || res.message || 'An unexpected error occurred';

            const cleanMessage = rawMessage.replace(/;/g, '');

            this.dialogPopUp.error('Error', cleanMessage, 'OK');
          },
        });
      }
    }
  }
}
