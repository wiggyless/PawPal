import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  LoginCommand,
  LoginCommandDto,
  RefreshTokenCommand,
  RefreshTokenCommandDto,
  LogoutCommand,
  ConfirmEmailCommandDto,
  ResendConfirmationEmailCommand
} from './auth-api.model';

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/Auth`;
  private http = inject(HttpClient);

  /**
   * POST /Auth/login
   * Authenticate user and get access/refresh tokens.
   */
  login(payload: LoginCommand): Observable<LoginCommandDto> {
    return this.http.post<LoginCommandDto>(`${this.baseUrl}/login`, payload);
  }

  /**
   * POST /Auth/refresh
   * Refresh access token using refresh token.
   */
  refresh(payload: RefreshTokenCommand): Observable<RefreshTokenCommandDto> {
    return this.http.post<RefreshTokenCommandDto>(`${this.baseUrl}/refresh`, payload);
  }

  /**
   * POST /Auth/logout
   * Invalidate refresh token and logout user.
   */
  logout(payload: LogoutCommand): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/logout`, payload);
  }

  /**
 * GET /Auth/confirm-email
 * Confirm user email using token from email link.
 */
confirmEmail(token: string): Observable<ConfirmEmailCommandDto> {
  return this.http.get<ConfirmEmailCommandDto>(`${this.baseUrl}/confirm-email`, {
    params: { token }
  });
}
resendConfirmationEmail(email: string): Observable<ResendConfirmationEmailCommand> {
  return this.http.post<ResendConfirmationEmailCommand>(`${environment.apiUrl}/Users/resend-confirmation`, { email });
}
}