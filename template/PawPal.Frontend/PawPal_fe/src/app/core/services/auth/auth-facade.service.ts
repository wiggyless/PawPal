import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, tap, catchError, map, switchMap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { NotificationService } from '../notifications/notification.service';
import { AuthApiService } from '../../../api-services/auth/auth-api.service';
import {
  LoginCommand,
  LoginCommandDto,
  LogoutCommand,
  RefreshTokenCommand,
  RefreshTokenCommandDto,
} from '../../../api-services/auth/auth-api.model';

import { AuthStorageService } from './auth-storage.service';
import { CurrentUserDto } from './current-user.dto';
import { JwtPayloadDto, NET_CLAIM_TYPES } from './jwt-payload.dto';

@Injectable({ providedIn: 'root' })
export class AuthFacadeService {
  private api = inject(AuthApiService);
  private storage = inject(AuthStorageService);
  private router = inject(Router);
  private notificationService = inject(NotificationService);
  private _currentUser = signal<CurrentUserDto | null>(null);

  currentUser = this._currentUser.asReadonly();
  isAuthenticated = computed(() => !!this._currentUser());

  roleid = computed(() => this.currentUser()?.roleid ?? 1);
  userId = computed(() => this.currentUser()?.userId ?? null);
  email = computed(() => this.currentUser()?.email ?? null);
  constructor() {
    this.initializeFromToken();
  }

  login(payload: LoginCommand): Observable<void> {
    return this.api.login(payload).pipe(
      tap((response: LoginCommandDto) => {
        this.storage.saveLogin(response);
        this.decodeAndSetUser(response.accessToken);
        this.notificationService.checkPermission();
      }),
      map(() => void 0),
    );
  }

  logout(): Observable<void> {
    const refreshToken = this.storage.getRefreshToken();

    return this.notificationService.clearToken().pipe(
      switchMap(() => {
        this.clearUserState();
        if (!refreshToken) {
          return of(void 0);
        }
        const payload: LogoutCommand = { refreshToken };
        return this.api.logout(payload).pipe(catchError(() => of(void 0)));
      }),
    );
  }

  refresh(payload: RefreshTokenCommand): Observable<RefreshTokenCommandDto> {
    return this.api.refresh(payload).pipe(
      tap((response: RefreshTokenCommandDto) => {
        this.storage.saveRefresh(response);
        this.decodeAndSetUser(response.accessToken);
      }),
    );
  }

  confirmEmail(token: string): Observable<void> {
    return this.api.confirmEmail(token).pipe(map(() => void 0));
  }

  confirmEmailChange(token: string): Observable<void> {
    return this.api.confirmEmailChange(token).pipe(map(() => void 0));
  }

  timeoutRefresh(payload: RefreshTokenCommand) {
    this.api.refresh(payload).subscribe((response) => {
      this.storage.saveRefresh(response);
      this.decodeAndSetUser(response.accessToken);
    });
  }

  redirectToLogin(): void {
    this.clearUserState();
    this.router.navigate(['/auth/login']);
  }

  getAccessToken(): string | null {
    return this.storage.getAccessToken();
  }

  getRefreshToken(): string | null {
    return this.storage.getRefreshToken();
  }

  private initializeFromToken(): void {
    const token = this.storage.getAccessToken();
    if (token) {
      this.decodeAndSetUser(token);
      this.notificationService.checkPermission();
    }
  }

  private decodeAndSetUser(token: string): void {
    try {
      const payload = jwtDecode<JwtPayloadDto>(token);
      const user: CurrentUserDto = {
        userId: Number(payload.sub),
        email: payload[NET_CLAIM_TYPES.Email],
        roleid: Number(payload.role_id),
        tokenVersion: Number(payload.ver),
      };

      this._currentUser.set(user);
    } catch (error) {
      console.error('Failed to decode JWT token:', error);
      this._currentUser.set(null);
    }
  }

  private clearUserState(): void {
    this._currentUser.set(null);
    this.storage.clear();
  }
  resendConfirmationEmail(email: string): Observable<void> {
    return this.api.resendConfirmationEmail(email).pipe(map(() => void 0));
  }
}
