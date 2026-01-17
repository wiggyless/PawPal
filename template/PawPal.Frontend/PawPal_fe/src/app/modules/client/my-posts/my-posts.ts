import { Component, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import {
  ListAnimal,
  listAnimalPostsByUserIdDto,
} from '../../../api-services/animal-posts/animal-posts.model';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { ListAnimalsDto } from '../../../api-services/animals/animal-model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-posts',
  standalone: false,
  templateUrl: './my-posts.html',
  styleUrl: './my-posts.scss',
})
export class MyPosts implements OnInit {
  currentUser: any;
  animalPostsService = inject(AnimalPostService);
  animalPostList: Observable<listAnimalPostsByUserIdDto[]> | undefined;
  envLink = environment;
  route = inject(Router);
  constructor(crr: CurrentUserService) {
    this.currentUser = crr;
  }
  ngOnInit(): void {
    this.loadAnimalPosts();
  }
  loadAnimalPosts(): void {
    const userObject = {
      userId: this.currentUser?.userId,
    };
    this.animalPostList = this.animalPostsService.listAnimalPostsByUserId(userObject);
  }
  loadPost(post: listAnimalPostsByUserIdDto): void {
    this.route.navigate(['/post'], {
      queryParams: {
        postID: post.postId,
        animalID: post.animalID,
        cityID: post.cityID,
        userID: post.userId,
      },
    });
  }
}
