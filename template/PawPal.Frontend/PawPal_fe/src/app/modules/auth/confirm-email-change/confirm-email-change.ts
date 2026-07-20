import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-confirm-email-change',
  standalone: false,
  templateUrl: './confirm-email-change.html',
  styleUrl: './confirm-email-change.scss',
})
export class ConfirmEmailChangeComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private auth = inject(AuthFacadeService);
  public router = inject(Router);

  status: 'loading' | 'success' | 'error' | 'expired' = 'loading';

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (!token) {
      this.status = 'error';
      return;
    }

    this.auth.confirmEmailChange(token).subscribe({
      next: () => {
        this.status = 'success';
        this.auth.logout().subscribe({
          complete: () => setTimeout(() => this.router.navigate(['/auth/login']), 3000),
        });
      },
      error: (err) => {
        const message = err?.error?.message ?? '';
        if (message.toLowerCase().includes('expired')) {
          this.status = 'expired';
        } else {
          this.status = 'error';
        }
      },
    });
  }
}
