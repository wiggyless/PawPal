import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateUserDisabled } from './users-disabled.model';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class UserDisabledService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/UsersDisabledHistory';

  createUserDisable(payload: CreateUserDisabled): Observable<{ id: number }> {
    console.log(payload);
    return this.http.post<{ id: number }>(this.apiUrl, payload);
  }
  deleteUserDisable(id: number): Observable<{ id: number }> {
    return this.http.delete<{ id: number }>(`${this.apiUrl}/${id}`);
  }
}
