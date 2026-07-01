import { Component, inject, Input, OnInit } from '@angular/core';
import { ReportedProblemService } from '../../../../api-services/moderation/reported-problem/reported-problem.service';
import { Subscriber, Subscription } from 'rxjs';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { FormsModule } from '@angular/forms';
import { DialoguePopupService } from '../../../../api-services/dialogue-popup/dialogue-popup.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './footer-component.html',
  styleUrl: './footer-component.scss',
})
export class FooterComponent implements OnInit {
  reportedProblemService = inject(ReportedProblemService);
  sub: Subscription | undefined;
  currentUserService = inject(CurrentUserService);
  dialogPopUp = inject(DialoguePopupService);
  description: string = '';
  ngOnInit(): void {}
  clearText(): void {
    const textarea = document.querySelector('.footer-left textarea') as HTMLTextAreaElement;
    if (textarea) {
      textarea.value = '';
      this.description = '';
    }
  }
  sendReport(): void {
    this.sub = this.reportedProblemService
      .createProblemReport({
        description: this.description,
        userID: this.currentUserService.userId() as number,
        dateSent: new Date(),
      })
      .subscribe({
        next: (res) => {
          this.dialogPopUp.success('Success', 'Report sent!', 'OK');
          this.clearText();
        },
        error: (res) => {
          this.clearText();
          this.dialogPopUp.success(res, 'OK');
        },
      });
  }
}
