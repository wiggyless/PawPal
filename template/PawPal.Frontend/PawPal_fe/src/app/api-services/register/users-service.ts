import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateUserCommand, GetByEmailQueryDto, GetByUsernameQueryDto } from './users-model';

@Injectable({
  providedIn: 'root'
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
}