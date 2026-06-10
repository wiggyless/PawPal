import { Component, signal } from '@angular/core';
import { initializeApp } from 'firebase/app';
import { environment } from '../environments/environment';
import { getAnalytics } from "firebase/analytics";


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

  constructor() {
    initializeApp(environment.firebase);
  }
}
