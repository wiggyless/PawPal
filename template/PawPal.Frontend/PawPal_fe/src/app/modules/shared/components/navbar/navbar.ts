import { Component, inject, Input, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PublicRoutingModule } from '../../../public/public-routing-module';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
  imports:[PublicRoutingModule, CommonModule]
})
export class NavbarComponent{
    currentUser = inject(CurrentUserService);
    @Input() roleId: number | null = null;
    menuOpened = false;
    toggleMenu(): void{
      this.menuOpened=!this.menuOpened;
    }
}
