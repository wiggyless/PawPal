import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthTimeoutService } from './core/services/auth/auth-timeout.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.scss',
})
export class App implements OnInit {
  protected readonly title = signal('PawPal_fe');
  private authTimeout = inject(AuthTimeoutService);

  ngOnInit(): void {
    this.authTimeout.startExpirationTracker();
  }

}