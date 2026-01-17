import { Component, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AnimalUserService } from '../../../api-services/animal-users/animal-users-service';
import { LogoutComponent } from '../../auth/logout/logout/logout';
import { Router } from '@angular/router';
import { AuthApiService } from '../../../api-services/auth/auth-api.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-dialogue-component',
  standalone: false,
  templateUrl: './dialogue-component.html',
  styleUrl: './dialogue-component.scss',
})
export class DialogueComponent implements OnInit {
  userService= inject(CurrentUserService);
  userActualService = inject(AnimalUserService);
  auth = inject(AuthApiService);
  router = inject(Router);
  dialog = inject(MatDialog);
  userId : any;
  ngOnInit(): void {
    this.userId = this.userService.userId;
    console.log(this.userId);
  }
  onDelete():void{
    this.userActualService.deleteUser(this.userId).subscribe({
      next: (res)=>{
        console.log('Worked', res)
        this.dialog.closeAll();
        this.router.navigate(['/auth/logout']);
      },
      error:(res)=>{
        console.log('Poop', res)
      }
    });
  }
}
