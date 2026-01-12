import { Component, inject, Input, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PublicRoutingModule } from '../../../public/public-routing-module';
import { CommonModule } from '@angular/common';
import { ClientModule } from '../../../client/client-module';
import { SharedModule } from 'primeng/api';
@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
  imports: [PublicRoutingModule, SharedModule],
})
export class NavbarComponent {
  currentUser = inject(CurrentUserService);
  @Input() roleId: number | null = null;
  menuOpened = false;
  toggleMenu(): void {
    this.menuOpened = !this.menuOpened;
  }
}
