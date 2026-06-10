import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { PublicRoutingModule } from '../../../public/public-routing-module';
import { CommonModule } from '@angular/common';
import { ClientModule } from '../../../client/client-module';
import { SharedModule } from 'primeng/api';
import { DyanmicThemeService } from '../../../../core/services/dynamic-theme.service';
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
  dynamicThemeService = inject(DyanmicThemeService);
  menuOpened = false;
  tipsMenuOpen = false;
  aboutUsMenuOpen = false;
  newsMenuOpen = false;
  theme: string = 'light';
  cd = inject(ChangeDetectorRef);
  toggleMenu(): void {
    this.menuOpened = !this.menuOpened;
  }

  toggleTipsMenu(): void {
    this.tipsMenuOpen = !this.tipsMenuOpen;
    this.aboutUsMenuOpen = false;
    this.newsMenuOpen = false;
  }

  toggleAboutUsMenu(): void {
    this.aboutUsMenuOpen = !this.aboutUsMenuOpen;
    this.tipsMenuOpen = false;
    this.newsMenuOpen = false;
  }

  toggleNewsMenu(): void {
    this.newsMenuOpen = !this.newsMenuOpen;
    this.tipsMenuOpen = false;
    this.aboutUsMenuOpen = false;
  }
  toggleTheme(): void {
    this.dynamicThemeService.toggleTheme();
  }
}
