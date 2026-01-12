import { Component, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { inject } from '@angular/core/primitives/di';
import { NavbarComponent } from '../../../shared/components/navbar/navbar';
import { RouterOutlet } from '@angular/router';
import { SharedModule } from 'primeng/api';
@Component({
  selector: 'app-client-layout',
  standalone: false,
  templateUrl: './client-layout.html',
  styleUrl: './client-layout.scss',
})
export class ClientLayout {
  currentUser: any;
  constructor(crr: CurrentUserService) {
    this.currentUser = crr;
  }
}
