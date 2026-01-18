import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  ViewEncapsulation,
  inject,
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AnimalsHealthService } from '../../../../api-services/animals-health/animals-health-service';
import { GetAnimalsHealthByIdDto } from '../../../../api-services/animals-health/animals-health-model';
import { AnimalService } from '../../../../api-services/animals/animal';
import { CitiesService } from '../../../../api-services/cities/cities.service';
import { GetCityByIdDto } from '../../../../api-services/cities/cities.model';
import { AnimalUserService } from '../../../../api-services/animal-users/animal-users-service';
import { PostImagesService } from '../../../../api-services/animal-post-images/animal-post-images-service';
import { environment } from '../../../../../environments/environment';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { catchError, Observable, of } from 'rxjs';
import { GetPostImageById } from '../../../../api-services/animal-post-images/animal-post-images-model';
import { AnimalPostService } from '../../../../api-services/animal-posts/animal-posts.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogueComponent } from '../../../client/dialogue-component/dialogue-component';
@Component({
  selector: 'app-post',
  standalone: false,
  templateUrl: './post.html',
  styleUrl: './post.scss',
  encapsulation: ViewEncapsulation.None,
})
export class PostComponent implements OnInit {
  // routing info holders
  animalId: number = 0;
  cityId: number = 0;
  userId: number = 0;
  postId: number = 0;

  // injections

  route = inject(ActivatedRoute);
  routeNext = inject(Router);
  currentUser = inject(CurrentUserService);
  animalHealthService = inject(AnimalsHealthService);
  animalService = inject(AnimalService);
  cityService = inject(CitiesService);
  userService = inject(AnimalUserService);
  postImageService = inject(PostImagesService);
  postService = inject(AnimalPostService);
  nextRoute = inject(Router);
  cd = inject(ChangeDetectorRef);
  dialog = inject(MatDialog);
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
    hasPapers: false,
  };
  user = {
    firstName: '',
    lastName: '',
    dateTime: '',
  };
  city: GetCityByIdDto = {
    id: 0,
    name: '',
  };

  // random variables
  postImage: any;
  dateAdded: string = '';
  imagesList: Observable<string[]> | undefined;
  env = environment;

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.route.queryParams.subscribe((params) => {
      this.postId = params['postID'];
      this.animalId = params['animalID'];
      this.cityId = params['cityID'];
      this.userId = params['userID'];
      this.dateAdded = params['dateAdded'];
    });
    this.loadAnimal();
    this.loadHealth();
    this.loadCity();
    this.loadUsers();
    this.loadPostImages();
  }

  keepOrder = (a: any, b: any) => 0;
  loadHealth(): void {
    this.animalHealthService.getAnimalHealthHistoryById(this.animalId).subscribe((response) => {
      this.animalHealth = response;
    });
  }
  loadAnimal(): void {
    this.animalService.getAnimalById(this.animalId as number).subscribe((response) => {
      let sourceKeys = Object.keys(response);
      sourceKeys.forEach((key) => {
        if (key in this.animal) {
          (this.animal as any)[key] = (response as any)[key];
        }
      });
    });
  }
  loadCity(): void {
    this.cityService.getCityById(this.cityId).subscribe((response) => {
      this.city = response;
    });
  }
  loadUsers(): void {
    this.userService.getUser(this.userId).subscribe((response) => {
      this.user = response;
    });
  }
  loadPostImages(): void {
    this.imagesList = this.postImageService.getImagePost(this.postId).pipe(
      catchError((error) => {
        console.error('Error fetching images:', error);
        return of([]);
        // implement stock image for when the image is not found
      }),
    );

    /*.subscribe((response) => {
      = response.postImages;
      this.cd.detectChanges();
    });
    */
  }

  routeEditPost(): void {
    this.routeNext.navigate(['/client/my-profile/create-post'], {
      queryParams: {
        postID: this.postId,
        update: true,
        animalID: this.animalId,
      },
    });
  }
  deletePost() {
    console.log(this.postId);
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = {postDelete: true, profileDelete:false, postId: this.postId, animalId: this.animalId};
    this.dialog.open(DialogueComponent,dialogConfig);
  }
}
