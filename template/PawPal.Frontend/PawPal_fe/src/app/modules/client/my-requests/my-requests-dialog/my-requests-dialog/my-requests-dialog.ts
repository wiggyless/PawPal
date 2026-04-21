import { DialogRef } from '@angular/cdk/dialog';
import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AnimalRequirementService } from '../../../../../api-services/animals-requirements/animals-requirements-service';
import { GetAdoptionRequirementsById } from '../../../../../api-services/animals-requirements/animals-requirements-model';
import { forkJoin, Subscription } from 'rxjs';
import { AnimalPostService } from '../../../../../api-services/animal-posts/animal-posts.service';
import { UserService } from '../../../../../api-services/register/users-service';
import { AnimalUserService } from '../../../../../api-services/animal-users/animal-users-service';
import { AnimalPostByIdQuery } from '../../../../../api-services/animal-posts/animal-posts.model';
import { GetUserByIdDto } from '../../../../../api-services/animal-users/animal-users-model';
import { AnimalRequestService } from '../../../../../api-services/animals-adoption/animals-adoption-service';
import { UpdateRequestByID } from '../../../../../api-services/animals-adoption/animals-adoption-model';
import { CurrentUserService } from '../../../../../core/services/auth/current-user.service';
@Component({
  selector: 'app-my-requests-dialog',
  standalone: false,
  templateUrl: './my-requests-dialog.html',
  styleUrl: './my-requests-dialog.scss',
})
export class MyRequestsDialog implements OnInit, OnDestroy {
  dialogReg = inject(MatDialogRef);
  dialogData = inject(MAT_DIALOG_DATA);
  postAPI = inject(AnimalPostService);
  userAPI = inject(AnimalUserService);
  reqAPI = inject(AnimalRequirementService);
  requestService = inject(AnimalRequestService);
  reqID: number = 0;
  postID: number = 0;
  status: string = '';
  cityCantonName: string = '';
  sentDate: Date = new Date();
  sentDateString = '';
  isAnotherUser = false;
  requestID: number = 0;
  reqData: GetAdoptionRequirementsById | undefined;
  user: GetUserByIdDto | undefined;
  currentUser = inject(CurrentUserService);
  fullAddress: string = '';

  cd = inject(ChangeDetectorRef);

  updateRequest: UpdateRequestByID = {
    requestID: 0,
    status: '',
  };
  private mySubscription?: Subscription;
  private updateSubcription?: Subscription;
  isLoaded = false;
  ngOnInit(): void {
    this.reqID = this.dialogData.reqID;
    this.postID = this.dialogData.postID;
    this.status = this.dialogData.status;
    this.cityCantonName = this.dialogData.cityCantonName;
    this.sentDate = this.dialogData.sentDate;
    this.sentDateString = new Date(this.sentDate).toISOString().replace('T', ' ').split('.')[0];
    this.requestID = this.dialogData.requestID;
    this.isAnotherUser = this.dialogData.isAnotherUser;
    this.loadReq();
  }
  ngOnDestroy(): void {
    this.mySubscription?.unsubscribe();
    this.updateSubcription?.unsubscribe();
  }
  loadReq() {
    this.mySubscription = forkJoin({
      // will have to change this - temp solution
      post: this.postAPI.getPostById(this.postID),
      request: this.reqAPI.getAnimalRequirementsById(this.reqID),
    }).subscribe({
      next: (reponse) => {
        this.reqData = reponse.request;
        this.fullAddress = `${this.reqData.address}, Floor ${this.reqData.floorNumber}`;
        console.log(this.reqData);
        this.isLoaded = true;
        this.userAPI.getUser(reponse.post.userID).subscribe((reponse) => {
          this.user = reponse;
          this.isLoaded = true;
          this.cd.detectChanges();
        });
      },
    });
  }

  // will have to change this later

  closeDialog() {
    this.dialogReg.close();
  }
  rejectRequest() {
    this.updateRequest.requestID = this.requestID;
    this.updateRequest.status = 'Rejected';
    this.updateSubcription = this.requestService
      .updateRequest(this.updateRequest)
      .subscribe((response) => {
        this.dialogReg.close();
      });
  }
  approveRequest() {
    throw new Error('Method not implemented.');
  }
}
