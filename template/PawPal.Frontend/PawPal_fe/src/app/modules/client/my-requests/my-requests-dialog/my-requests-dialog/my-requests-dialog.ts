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
import { GetUserByIdDto } from '../../../../../api-services/users/users-model';
import { Router } from '@angular/router';

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
      post: this.postAPI.getPostById(this.postID),
      request: this.reqAPI.getAnimalRequirementsById(this.reqID),
      adoptionRequest: this.requestService.getAnimalRequestById(this.requestID),
    }).subscribe({
      next: (reponse) => {
        this.reqData = reponse.request;
        this.fullAddress = `${this.reqData.address}, Floor ${this.reqData.floorNumber}`;
        this.isLoaded = true;

        this.userAPI.getUser(reponse.adoptionRequest.userId).subscribe((userResponse) => {
          this.user = userResponse;
          this.isLoaded = true;
          this.cd.detectChanges();
        });
      },
    });
  }

  closeDialog() {
    this.dialogReg.close();
  }
  rejectRequest() {
    this.updateRequest.requestID = this.requestID;
    this.updateRequest.status = 'Denied';
    this.updateSubcription = this.requestService.updateRequest(this.updateRequest).subscribe(() => {
      this.dialogReg.close(true);
    });
  }
  approveRequest() {
    this.updateRequest.requestID = this.requestID;
    this.updateRequest.status = 'Accepted';
    this.updateSubcription = this.requestService.updateRequest(this.updateRequest).subscribe(() => {
      this.dialogReg.close(true);
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
