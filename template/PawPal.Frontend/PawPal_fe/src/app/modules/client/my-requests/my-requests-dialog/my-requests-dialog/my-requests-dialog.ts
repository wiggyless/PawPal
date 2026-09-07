import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AnimalRequirementService } from '../../../../../api-services/animals-requirements/animals-requirements-service';
import { GetAdoptionRequirementsById } from '../../../../../api-services/animals-requirements/animals-requirements-model';
import { forkJoin, Subscription } from 'rxjs';
import { AnimalPostService } from '../../../../../api-services/animal-posts/animal-posts.service';
import { UserService } from '../../../../../api-services/users/users-service';
import { AnimalRequestService } from '../../../../../api-services/animals-adoption/animals-adoption-service';
import { UpdateRequestByID } from '../../../../../api-services/animals-adoption/animals-adoption-model';
import { CurrentUserService } from '../../../../../core/services/auth/current-user.service';
import { GetPublicUserProfileDto } from '../../../../../api-services/users/users-model';
import { Router } from '@angular/router';
import { environment } from '../../../../../../environments/environment';
import { DialoguePopupService } from '../../../../../api-services/dialogue-popup/dialogue-popup.service';

@Component({
  selector: 'app-my-requests-dialog',
  standalone: false,
  templateUrl: './my-requests-dialog.html',
  styleUrl: './my-requests-dialog.scss',
})
export class MyRequestsDialog implements OnInit, OnDestroy {
  dialogReg = inject(MatDialogRef);
  routeNext = inject(Router);
  dialogData = inject(MAT_DIALOG_DATA);
  postAPI = inject(AnimalPostService);
  userAPI = inject(UserService);
  reqAPI = inject(AnimalRequirementService);
  requestService = inject(AnimalRequestService);
  reqID: number = 0;
  postID: number = 0;
  status: string = '';
  cityCantonName: string = '';
  sentDate: Date = new Date();
  isAnotherUser = false;
  canModerate = false;
  requestID: number = 0;
  reqData: GetAdoptionRequirementsById | undefined;
  user: GetPublicUserProfileDto | undefined;
  currentUser = inject(CurrentUserService);
  fullAddress: string = '';
  env = environment.apiUrl;
  cd = inject(ChangeDetectorRef);

  updateRequest: UpdateRequestByID = {
    requestID: 0,
    status: '',
  };
  private mySubscription?: Subscription;
  private updateSubcription?: Subscription;
  dialogPopUp = inject(DialoguePopupService);
  isLoaded = false;
  ngOnInit(): void {
    this.reqID = this.dialogData.reqID;
    this.postID = this.dialogData.postID;
    this.status = this.dialogData.status;
    this.cityCantonName = this.dialogData.cityCantonName;
    this.sentDate = this.dialogData.sentDate;
    this.requestID = this.dialogData.requestID;
    this.isAnotherUser = this.dialogData.isAnotherUser;
    this.canModerate = this.dialogData.canModerate ?? false;
    this.loadReq();
  }
  ngOnDestroy(): void {
    this.mySubscription?.unsubscribe();
    this.updateSubcription?.unsubscribe();
  }
  loadReq() {
    this.mySubscription = forkJoin({
      post: this.postAPI.getPostById(this.postID),
      request: this.reqAPI.getAnimalRequirementsById(this.reqID),
      adoptionRequest: this.requestService.getAnimalRequestById(this.requestID),
    }).subscribe({
      next: (reponse) => {
        this.reqData = reponse.request;
        this.fullAddress = `${this.reqData.address}, Floor ${this.reqData.floorNumber}`;
        this.isLoaded = true;

        this.userAPI.getPublicProfile(reponse.adoptionRequest.userId).subscribe({
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
        this.dialogReg.close(false);
      },
    });
  }

  closeDialog() {
    this.dialogReg.close();
  }
  rejectRequest() {
    this.updateRequest.requestID = this.requestID;
    this.updateRequest.status = 'Denied';
    this.updateSubcription = this.requestService.updateRequest(this.updateRequest).subscribe({
      next: () => {
        this.dialogPopUp.success('Request Denied', 'The adoption request has been denied.', 'OK');
        this.dialogReg.close(true);
      },
      error: (err) => {
        this.dialogPopUp.error(
          'Error',
          err?.error?.message ?? 'Could not deny the request. Please try again.',
          'OK',
        );
        this.dialogReg.close(false);
      },
    });
  }
  approveRequest() {
    this.updateRequest.requestID = this.requestID;
    this.updateRequest.status = 'Accepted';
    this.updateSubcription = this.requestService.updateRequest(this.updateRequest).subscribe({
      next: () => {
        this.dialogPopUp.success(
          'Request Approved',
          'The adoption request has been approved and the animal is now marked as adopted.',
          'OK',
        );
        this.dialogReg.close(true);
      },
      error: (err) => {
        this.dialogPopUp.error(
          'Error',
          err?.error?.message ?? 'Could not approve the request. Please try again.',
          'OK',
        );
        this.dialogReg.close(false);
      },
    });
  }
  routeMessage(): void {
    this.dialogReg.close();
    if (this.currentUser.getDefaultRoute() == '/login') {
      this.routeNext.navigate(['login']);
    } else {
      this.routeNext.navigate(['/client/messages'], {
        queryParams: {
          recipientId: this.user?.id,
        },
      });
    }
  }
}
