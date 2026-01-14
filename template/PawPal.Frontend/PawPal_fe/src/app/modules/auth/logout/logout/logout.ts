import { Component, inject, OnInit } from '@angular/core';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
@Component({
  selector: 'app-logout',
  standalone: false,
  templateUrl: './logout.html',
  styleUrl: './logout.scss',
})
export class LogoutComponent implements OnInit{
  private authService = inject(AuthFacadeService);
  private router = inject(Router);
 countdownSeconds = 3;

  ngOnInit(): void {
    // Call logout (handles API call + clears state)
    this.authService.logout().subscribe({
      next: () => this.startCountdown(),
      error: () => this.startCountdown() // Even if API fails, clear local state
    });
  }

  private startCountdown(): void {
    const intervalId = setInterval(() => {
      this.countdownSeconds--;

      if (this.countdownSeconds <= 0) {
        clearInterval(intervalId);
        this.router.navigate(['']);
      }
    }, 1000);
  }
}
