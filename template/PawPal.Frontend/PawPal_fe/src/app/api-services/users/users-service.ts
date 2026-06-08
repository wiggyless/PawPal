import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateUserCommand,
  GetByEmailQueryDto,
  GetByUsernameQueryDto,
  GetUserByIdDto,
  UpdateUserCommand,
} from './users-model';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Users';
  createUser(command: CreateUserCommand): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}`, command);
  }

  getByUsername(username: string): Observable<GetByUsernameQueryDto> {
    return this.http.get<GetByUsernameQueryDto>(`${this.apiUrl}/lookup?username=${username}`);
  }

  getByEmail(email: string): Observable<GetByEmailQueryDto> {
    return this.http.get<GetByEmailQueryDto>(`${this.apiUrl}/lookup?email=${email}`);
  }

  getUser(request?: any): Observable<GetUserByIdDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<GetUserByIdDto>(`${this.apiUrl}/${request}`);
  }
  updateUser(id: number, payload: UpdateUserCommand): Observable<void> {
    const params = payload ? buildHttpParams(payload as any) : undefined;
    return this.http.put<void>(`${this.apiUrl}/${id}`, { params });
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
