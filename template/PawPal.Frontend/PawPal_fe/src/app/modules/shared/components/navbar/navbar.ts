import { Component, inject, Input } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PublicRoutingModule } from '../../../public/public-routing-module';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
  imports: [PublicRoutingModule],
})
export class NavbarComponent {
  currentUser = inject(CurrentUserService);
  @Input() roleId: number | null = null;
}
