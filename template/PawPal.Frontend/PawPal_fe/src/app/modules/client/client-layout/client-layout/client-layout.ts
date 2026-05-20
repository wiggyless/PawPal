import { Component, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
@Component({
  selector: 'app-client-layout',
  standalone: false,
  templateUrl: './client-layout.html',
  styleUrls: ['./client-layout.scss'],
})
export class ClientLayout {
  currentUser: any;
  constructor(crr: CurrentUserService) {
    this.currentUser = crr;
  }
}
