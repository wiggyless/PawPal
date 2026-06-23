import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { AnimalRequestService } from '../../../../api-services/animals-adoption/animals-adoption-service';
import {
  GetAdoptionRequestList,
  GetAdoptionRequestListQuery,
} from '../../../../api-services/animals-adoption/animals-adoption-model';
import { PageResult } from '../../../../core/models/paging/page-result';
import { forkJoin, Subscription } from 'rxjs';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { CantonsService } from '../../../../api-services/cantons/cantons-service';
import { AnimalService } from '../../../../api-services/animals/animal';
import { environment } from '../../../../../environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { GetMainImagePostBlobClass } from '../../../../api-services/animal-post-images/animal-post-images-model';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { MySentRequestDialog } from '../my-sent-request-dialog/my-sent-request-dialog/my-sent-request-dialog';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-my-requests',
  standalone: false,
  templateUrl: './my-sent-requests.html',
  styleUrl: './my-sent-requests.scss',
})
export class MySentRequests implements OnInit, OnDestroy {
  currentUser = inject(CurrentUserService);
  myRequestApi = inject(AnimalRequestService);
  animalPostsService = inject(AnimalPostService);
  cantonApi = inject(CantonsService);
  animalApi = inject(AnimalService);
  postImages = inject(PostImagesService);
  dialog = inject(MatDialog);
  sanitizer = inject(DomSanitizer);

  cantonsList: any = [];
  envLink = environment;
  isLoaded = signal(true);
  private mySubscription?: Subscription;
  private myPostSubscription?: Subscription;
  requestsList: PageResult<GetAdoptionRequestList> | undefined;
  listEmpty = signal(false);
  fullName: string = '';
  requestQuery: GetAdoptionRequestListQuery = {
    userID: this.currentUser.userId() as number,
    sent: true,
    paging: {
      page: 1,
      pageSize: 4,
    },
  };

  catalogImages: GetMainImagePostBlobClass[] = [];
  imagesLoaded = signal(true);

  tempList: number[] = [];
  ngOnInit(): void {
    this.loadRequest();
  }
  loadRequest() {
    this.myPostSubscription = forkJoin({
      request: this.myRequestApi.listAnimalRequests(this.requestQuery),
      cantons: this.cantonApi.listCantons(),
    }).subscribe({
      next: (response) => {
        this.requestsList = response.request;
        if (this.requestsList.items.length == 0) {
          this.listEmpty.set(true);
        }
        console.log(this.requestsList);
        this.cantonsList = response.cantons;
        this.imagesLoaded.set(true);
      },
    });
  }
  ngOnDestroy(): void {
    this.mySubscription?.unsubscribe();
    this.myPostSubscription?.unsubscribe();
  }
  loadRequestDialog(request: GetAdoptionRequestList) {
    this.dialog.open(MySentRequestDialog, {
      width: '70%',
      maxWidth: '90vw',
      maxHeight: '95vh',
      panelClass: 'transparent-dialog',
      data: {
        reqID: request.requirementId,
        cityCantonName: request.city + ',' + request.canton,
        sentDate: request.dateSent,
        status: request.status.toLocaleUpperCase(),
        postID: request.postID,
      },
    });
  }
  getPostImage(mainImage: string) {
    return this.sanitizer.bypassSecurityTrustUrl(this.envLink.apiUrl + mainImage);
  }
}
