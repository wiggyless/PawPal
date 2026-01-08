import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { GetUserByIdDto } from './animal-users-model';
@Injectable({
  providedIn: 'root',
})
export class AnimalUserService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Users';

  getUser(request?: any): Observable<GetUserByIdDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetUserByIdDto>(`${this.apiUrl}/${request}`);
  }
}
