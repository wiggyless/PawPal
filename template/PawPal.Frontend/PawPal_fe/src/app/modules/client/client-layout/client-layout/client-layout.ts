import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { Router, NavigationEnd, Event } from '@angular/router';
import { filter } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-client-layout',
  standalone: false,
  templateUrl: './client-layout.html',
  styleUrls: ['./client-layout.scss'],
})
export class ClientLayout implements OnInit {
  currentUser: any;
  selectedItem: string = 'My_Profile';
  private router = inject(Router);
  cd = inject(ChangeDetectorRef);
  constructor(crr: CurrentUserService) {
    this.currentUser = crr;
  }
  hasLoaded: boolean = false;
  ngOnInit(): void {
    this.selectedItem = this.router.url;
    console.log(this.selectedItem);
  }
}
