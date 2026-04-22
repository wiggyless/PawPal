import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { Dialog } from '@angular/cdk/dialog';
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
  listAnimalPostsByUserIdDto,
  ListPostsByRange,
} from '../../../../api-services/animal-posts/animal-posts.model';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { CantonsService } from '../../../../api-services/cantons/cantons-service';
import { AnimalBreedService } from '../../../../api-services/anima-breed/animal-breed.service';
import { AnimalService } from '../../../../api-services/animals/animal';
import { ListCantonsDto } from '../../../../api-services/cantons/cantons-model';
import { environment } from '../../../../../environments/environment';
import { MatDialog } from '@angular/material/dialog';
import {
  GetMainImagePostBlob,
  GetMainImagePostBlobClass,
} from '../../../../api-services/animal-post-images/animal-post-images-model';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { MyRequestsDialog } from '../../my-requests/my-requests-dialog/my-requests-dialog/my-requests-dialog';

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
  isLoaded = false;
  private mySubscription?: Subscription;
  private myPostSubscription?: Subscription;
  requestsList: PageResult<GetAdoptionRequestList> | undefined;
  fullName: string = '';
  requestQuery: GetAdoptionRequestListQuery = {
    userID: this.currentUser.userId as number,
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
        this.loadPostImages(this.requestsList.items);
        console.log(response.request);
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
  loadPostImages(idList: GetAdoptionRequestList[]): void {
    idList.forEach((element) => {
      this.tempList.push(element.postID);
    });
    console.log(idList);
    this.postImages.getMainImagePostBlob(this.tempList).subscribe((response) => {
      this.setImages(response);
    });
  }
  setImages(blobList: GetMainImagePostBlob[]): void {
    blobList.forEach((x) => {
      if (x.mainImage != '') {
        const byteCharacters = atob(x.mainImage);
        const byteNumbers = new Array(byteCharacters.length);

        for (let i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        let mimeType = 'image/png';
        if (byteArray.length > 4) {
          const header = byteArray.slice(0, 4);
          let headerHex = '';
          for (let i = 0; i < header.length; i++) {
            headerHex += header[i].toString(16).toUpperCase();
          }
          if (headerHex.startsWith('89504E47')) {
            mimeType = 'image/png';
          } else if (headerHex.startsWith('FFD8FF')) {
            mimeType = 'image/jpeg';
          }
        }
        // 2. Create the Blob from the typed array
        const blob = new Blob([byteArray], { type: mimeType });

        // 3. Create the URL and add to form
        const imageUrl = URL.createObjectURL(blob);
        this.catalogImages.push(new GetMainImagePostBlobClass(x.postID, imageUrl));
      }
    });
    this.imagesLoaded = true;
    this.cd.detectChanges();
  }
  getPostImage(index: number) {
    return this.catalogImages.find((x) => x.postID == index)?.mainImage;
  }
}
