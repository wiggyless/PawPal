import { Component, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalPostService } from '../../../api-services/animal-posts/animal-posts.service';
import { listAnimalPostsByUserIdDto } from '../../../api-services/animal-posts/animal-posts.model';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-my-posts',
  standalone: false,
  templateUrl: './my-posts.html',
  styleUrl: './my-posts.scss',
})
export class MyPosts implements OnInit {
  currentUser: any;
  animalPostsService = inject(AnimalPostService);
  animalPostList: any = [];
  envLink = environment;
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
    this.animalPostsService.listAnimalPostsByUserId(userObject).subscribe((response) => {
      this.animalPostList = response;
      console.log(response);
    });
  }
}
