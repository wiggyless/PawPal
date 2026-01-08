import { Component, inject, OnInit } from '@angular/core';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';

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
    console.log("Logout begin!");
    this.authService.logout().subscribe({
      next: () => this.startCountdown(),
      error: () => this.startCountdown() // Even if API fails, clear local state
    });
  }

  private startCountdown(): void {
    const intervalId = setInterval(() => {
      this.countdownSeconds--;

      console.log("Brojimo.");

      if (this.countdownSeconds <= 0) {
        clearInterval(intervalId);
        this.router.navigate(['']);
      }
    }, 1000);
  }
}
