import { Component, OnInit, inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthFacadeService } from "../../../core/services/auth/auth-facade.service";

@Component({
  selector: 'app-confirm-email',
  standalone: false,
  templateUrl: './confirm-email.html',
  styleUrl: './confirm-email.scss'
})


export class ConfirmEmailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private auth = inject(AuthFacadeService);
  public router = inject(Router);

  status: 'loading' | 'success' | 'error' = 'loading';

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (!token) {
      this.status = 'error';
      return;
    }

    this.auth.confirmEmail(token).subscribe({
      next: () => {
        this.status = 'success';
        setTimeout(() => this.router.navigate(['/auth/login']), 3000);
      },
      error: () => {
        this.status = 'error';
      }
    });
  }
}