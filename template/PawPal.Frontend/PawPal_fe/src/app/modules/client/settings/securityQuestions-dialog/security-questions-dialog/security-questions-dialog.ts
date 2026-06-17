import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { SecurityQuestionService } from '../../../../../api-services/security/questions/questions-service';
import { GetSecurityQuestionDTO } from '../../../../../api-services/security/questions/questions-model';
import { DialogRef } from '@angular/cdk/dialog';
import { GetAndPostAnswerDTO } from '../../../../../api-services/security/answers/answer-model';
import { CurrentUserService } from '../../../../../core/services/auth/current-user.service';
import { SecurityAnswerService } from '../../../../../api-services/security/answers/answer-service';
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
  private _formBuilder = inject(FormBuilder);

  securityFormGroup = this._formBuilder.group({
    firstQuestion: [0, Validators.required],
    firstAnswer: ['', Validators.required],
    secondQuestion: [0, Validators.required],
    secondAnswer: ['', Validators.required],
    thirdQuestion: [0, Validators.required],
    thirdAnswer: ['', Validators.required],
  });
  dialogRef = inject(DialogRef);
  ngOnInit(): void {
    this.securityQuestions.getSecurityQuestionsByEmail().subscribe((response) => {
      this.securityQuestionList = response.items;
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
      this.answerService.createSecurityAnswer(anwserDTO).subscribe({
        next: (res) => {
          console.log('Answers saved');
          this.dialogRef.close();
        },
        error: (res) => {
          console.log('THRE WAS AN ERROR');
        },
      });
    }
  }
}
