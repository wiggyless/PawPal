import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
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
import { MyRequestsDialog } from '../../my-requests/my-requests-dialog/my-requests-dialog/my-requests-dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-my-requests',
  standalone: false,
  templateUrl: './request-history.html',
  styleUrl: './request-history.scss',
})
export class RequestHistory implements OnInit, OnDestroy {
  currentUser = inject(CurrentUserService);
  myRequestApi = inject(AnimalRequestService);
  animalPostsService = inject(AnimalPostService);
  cantonApi = inject(CantonsService);
  animalApi = inject(AnimalService);
  postImages = inject(PostImagesService);

  cd = inject(ChangeDetectorRef);
  cantonsList: any = [];
  envLink = environment;
  sanitizer = inject(DomSanitizer);
  isLoaded = false;
  private mySubscription?: Subscription;
  private myPostSubscription?: Subscription;
  requestsList: PageResult<GetAdoptionRequestList> | undefined;
  fullName: string = '';
  requestQuery: GetAdoptionRequestListQuery = {
    userID: this.currentUser.userId() as number,
    sent: false,
    paging: {
      page: 1,
      pageSize: 4,
    },
  };
  dialog = inject(MatDialog);
  catalogImages: GetMainImagePostBlobClass[] = [];
  imagesLoaded = signal(false);
  tempList: number[] = [];
  ngOnInit(): void {
    this.loadRequest();
  }
  loadRequest() {
    this.myPostSubscription = forkJoin({
      request: this.myRequestApi.listAnimalRequestsHistory(this.requestQuery),
      cantons: this.cantonApi.listCantons(),
    }).subscribe({
      next: (response) => {
        this.requestsList = response.request;
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
    this.dialog.open(MyRequestsDialog, {
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
  getPostImage(imagePath: string) {
    return this.sanitizer.bypassSecurityTrustUrl(this.envLink.apiUrl + imagePath);
  }
  handlePageEvent(event: PageEvent) {
    this.requestQuery.paging.page = event.pageIndex + 1;
    this.requestQuery.paging.pageSize = event.pageSize;
    this.loadRequest();
  }
}
