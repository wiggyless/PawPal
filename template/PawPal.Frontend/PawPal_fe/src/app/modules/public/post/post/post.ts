import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
  inject,
  signal,
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AnimalsHealthService } from '../../../../api-services/animals-health/animals-health-service';
import { GetAnimalsHealthByIdDto } from '../../../../api-services/animals-health/animals-health-model';
import { AnimalService } from '../../../../api-services/animals/animal';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { GetCityByIdDto } from '../../../../api-services/cities/cities.model';
import { UserService } from '../../../../api-services/users/users-service';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { environment } from '../../../../../environments/environment';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { forkJoin, Observable, Subscription } from 'rxjs';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { MatDialog } from '@angular/material/dialog';
import { SafeUrl } from '@angular/platform-browser';
import { ReportPostComponent } from '../../../client/report-post-component/report-post-component';
import { ReportUserComponent } from '../../../client/report-user-component/report-user-component/report-user-component';
import { GetUserByIdDto } from '../../../../api-services/users/users-model';
import { DialoguePopupComponent } from '../../../shared/components/dialogue-popup/dialogue-popup.component';

@Component({
  selector: 'app-post',
  standalone: false,
  templateUrl: './post.html',
  styleUrl: './post.scss',
  encapsulation: ViewEncapsulation.None,
})
export class PostComponent implements OnInit, OnDestroy {
  animalId: number = 0;
  cityId: number = 0;
  userId: number = 0;
  postId: number = 0;
  currentImageIndex = 0;

  route = inject(ActivatedRoute);
  routeNext = inject(Router);
  currentUser = inject(CurrentUserService);
  animalHealthService = inject(AnimalsHealthService);
  animalService = inject(AnimalService);
  cityService = inject(CitiesService);
  userService = inject(UserService);
  postImageService = inject(PostImagesService);
  postService = inject(AnimalPostService);
  cd = inject(ChangeDetectorRef);
  dialog = inject(DialoguePopupComponent);
  reportDialog = inject(MatDialog);
  postSub: Subscription | undefined;
  animalSub: Subscription | undefined;
  togetherSub: Subscription | undefined;
  
  animalHealth: GetAnimalsHealthByIdDto = {
    animalHealthHistoryId: 0,
    animalId: 0,
    vaccinated: false,
    parasiteFree: false,
    spayedOrNeutered: false,
    dietaryRestrictions: '',
    animalAllergies: [],
    animalDisabilities: [],
  };

  animal = {
    name: '',
    category: '',
    breed: '',
    gender: '',
    age: 0,
    has_Papers: false,
  };
  router = inject(Router);
  user: GetUserByIdDto | undefined;

  city: GetCityByIdDto = {
    id: 0,
    name: '',
  };

  postImage: any;
  dateAdded: Date = new Date();
  imagesList: Observable<string[]> | undefined;
  env = environment;
  objectUrl: string | null = null;
  imageUrl = signal<SafeUrl | null>(null);
  isCommentsLoaded = signal(false);
  isImagesLoaded = signal(false);
  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.route.queryParams.subscribe((params) => {
      this.postId = params['postID'];
    });

    this.imagesList = this.postImageService.getImagePost(this.postId);
    this.postSub = this.postService.getPostById(this.postId).subscribe((res) => {
      this.dateAdded = res.dateAdded;
      this.animalId = res.animalID;
      this.animalSub = this.animalService.getAnimalById(res.animalID).subscribe((resA) => {
        this.togetherSub = forkJoin({
          health: this.animalHealthService.getAnimalHealthHistoryById(this.animalId),
          cities: this.cityService.getCityById(res.cityID),
          users: this.userService.getUser(res.userID),
        }).subscribe({
          next: (response) => {
            let sourceKeys = Object.keys(resA);
            sourceKeys.forEach((key) => {
              if (key in this.animal) {
                (this.animal as any)[key] = (resA as any)[key];
              }
            });
            this.animalHealth = response.health;
            this.city = response.cities;
            this.user = response.users;
            this.isImagesLoaded.set(true);
          },
        });
      });
    });
  }
  ngOnDestroy(): void {
    this.animalSub?.unsubscribe();
    this.postSub?.unsubscribe();
    this.togetherSub?.unsubscribe();
  }
  keepOrder = (a: any, b: any) => 0;

  routeEditPost(): void {
    this.routeNext.navigate(['/client/create-post'], {
      queryParams: {
        postID: this.postId,
        update: true,
        animalID: this.animalId,
        dateAdded: this.dateAdded,
      },
    });
  }

  deletePost() {
    this.dialog.service.warning(
      'Delete Post',
      'Are you sure you want to delete this post?',
      'Yes, delete it',
      'Cancel',
      () => {
        this.postService.deletePost(this.postId, this.animalId).subscribe({
          next: () => {
            this.dialog.service.success('Post Deleted', 'The post has been deleted successfully.');
            this.routeNext.navigate(['/catalog']);
          },
        });
      },
    );
  }

  routeAdopt(): void {
    if (this.currentUser.getDefaultRoute() == '/login') {
      this.routeNext.navigate(['/auth/login']);
    } else {
      this.routeNext.navigate(['/client/adoption'], {
        queryParams: {
          postID: this.postId,
        },
      });
    }
  }

  nextImage(length: number): void {
    this.currentImageIndex = this.currentImageIndex === length - 1 ? 0 : this.currentImageIndex + 1;
  }
  prevImage(length: number): void {
    this.currentImageIndex = this.currentImageIndex === 0 ? length - 1 : this.currentImageIndex - 1;
  }

  getTransformStyle(): string {
    return `translateX(-${this.currentImageIndex * 100}%)`;
  }
  onCommentsLoaded() {
    this.isCommentsLoaded.set(true);
    this.cd.detectChanges();
  }

  routeMessage(): void {
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
  openReportDialog(): void {
    this.reportDialog.open(ReportPostComponent, {
      data: { postId: this.postId, userId: this.currentUser.userId() },
    });
  }
  openReportUserDialog(): void {
    this.reportDialog.open(ReportUserComponent, {
      data: { reportedUserID: this.user?.id, reportSentByID: this.currentUser.userId() },
    });
  }
  routeToProfile(id: number): void {
    this.router.navigate(['profile'], {
      queryParams: {
        userID: id,
      },
    });
  }
}
