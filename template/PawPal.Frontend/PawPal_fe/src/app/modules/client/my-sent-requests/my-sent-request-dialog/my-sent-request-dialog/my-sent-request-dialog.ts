import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AnimalRequirementService } from '../../../../../api-services/animals-requirements/animals-requirements-service';
import { GetAdoptionRequirementsById } from '../../../../../api-services/animals-requirements/animals-requirements-model';
import { forkJoin, Subscription } from 'rxjs';
import { AnimalPostService } from '../../../../../api-services/animal-posts/animal-posts.service';
import { UserService } from '../../../../../api-services/users/users-service';
import { GetPublicUserProfileDto } from '../../../../../api-services/users/users-model';
import { environment } from '../../../../../../environments/environment';
import { DialoguePopupService } from '../../../../../api-services/dialogue-popup/dialogue-popup.service';
@Component({
  selector: 'app-my-requests-dialog',
  standalone: false,
  templateUrl: './my-sent-request-dialog.html',
  styleUrl: './my-sent-request-dialog.scss',
})
export class MySentRequestDialog implements OnInit, OnDestroy {
  dialogReg = inject(MatDialogRef);
  dialogData = inject(MAT_DIALOG_DATA);
  postAPI = inject(AnimalPostService);
  userAPI = inject(UserService);
  reqAPI = inject(AnimalRequirementService);
  dialogPopUp = inject(DialoguePopupService);

  reqID: number = 0;
  postID: number = 0;
  status: string = '';
  cityCantonName: string = '';
  sentDate: Date = new Date();

  isAnotherUser = false;
  requestID: number = 0;
  reqData: GetAdoptionRequirementsById | undefined;
  user: GetPublicUserProfileDto | undefined;
  fullAddress: string = '';
  env = environment.apiUrl;
  cd = inject(ChangeDetectorRef);

  private mySubscription?: Subscription;
  isLoaded = false;
  ngOnInit(): void {
    this.reqID = this.dialogData.reqID;
    this.postID = this.dialogData.postID;
    this.status = this.dialogData.status;
    this.cityCantonName = this.dialogData.cityCantonName;
    this.sentDate = this.dialogData.sentDate;
    this.requestID = this.dialogData.requestID;
    this.isAnotherUser = this.dialogData.isAnotherUser;
    this.loadReq();
  }
  ngOnDestroy(): void {
    this.mySubscription?.unsubscribe();
  }
  loadReq() {
    this.mySubscription = forkJoin({
      post: this.postAPI.getPostById(this.postID),
      request: this.reqAPI.getAnimalRequirementsById(this.reqID),
    }).subscribe({
      next: (reponse) => {
        this.reqData = reponse.request;
        this.fullAddress = `${this.reqData.address}, Floor ${this.reqData.floorNumber}`;
        this.userAPI.getPublicProfile(reponse.post.userID).subscribe({
          next: (userResponse) => {
            this.user = userResponse;
            this.isLoaded = true;
            this.cd.detectChanges();
          },
          error: () => {
            this.isLoaded = true;
            this.cd.detectChanges();
          },
        });
      },
      error: (err) => {
        this.dialogPopUp.error(
          'Error',
          err?.error?.message ?? 'Could not load the request details. Please try again.',
          'OK',
        );
        this.dialogReg.close();
      },
    });
  }

  closeDialog() {
    this.dialogReg.close();
  }
}
