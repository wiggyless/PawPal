import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { Dialog } from '@angular/cdk/dialog';
import { MyRequestsDialog } from '../my-requests-dialog/my-requests-dialog/my-requests-dialog';
import { AnimalRequestService } from '../../../../api-services/animals-adoption/animals-adoption-service';
import {
  GetAdoptionRequestList,
  GetAdoptionRequestListQuery,
} from '../../../../api-services/animals-adoption/animals-adoption-model';
import { PageRequest } from '../../../../core/models/paging/page-request';
import { PageResult } from '../../../../core/models/paging/page-result';
import { forkJoin, Subscription } from 'rxjs';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import {
  ListAnimal,
  ListAnimalPostsByUserIdDto,
  ListPostsByRange,
} from '../../../../api-services/animal-posts/animal-posts.model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { CantonsService } from '../../../../api-services/cantons/cantons-service';
import { AnimalBreedService } from '../../../../api-services/animal-breed/animal-breed.service';
import { AnimalService } from '../../../../api-services/animals/animal';
import { ListCantonsDto } from '../../../../api-services/cantons/cantons-model';
import { environment } from '../../../../../environments/environment';
import { MatDialog } from '@angular/material/dialog';
import {
  GetMainImagePostBlob,
  GetMainImagePostBlobClass,
} from '../../../../api-services/animal-post-images/animal-post-images-model';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-my-requests',
  standalone: false,
  templateUrl: './my-requests.html',
  styleUrl: './my-requests.scss',
})
export class MyRequests implements OnInit, OnDestroy {
  currentUser = inject(CurrentUserService);
  myRequestApi = inject(AnimalRequestService);
  animalPostsService = inject(AnimalPostService);
  cantonApi = inject(CantonsService);
  animalApi = inject(AnimalService);
  postImages = inject(PostImagesService);

  cd = inject(ChangeDetectorRef);
  cantonsList: any = [];
  envLink = environment;
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
  imagesLoaded = false;
  tempList: number[] = [];
  sanitizer = inject(DomSanitizer);
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
        this.cantonsList = response.cantons;
        this.imagesLoaded = true;
        this.cd.detectChanges();
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
}
