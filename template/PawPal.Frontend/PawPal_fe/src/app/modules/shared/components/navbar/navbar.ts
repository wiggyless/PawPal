import { Component, inject, Input } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class NavbarComponent {
  currentUser = inject(CurrentUserService);
  @Input() roleId: number | null = null;
}
