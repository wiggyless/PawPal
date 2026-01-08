import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { GetAnimalsHealthByIdDto } from './animals-health-model';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class AnimalsHealthService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/AnimalHealthHistory';
  getAnimalHealthHistoryById(request?: any): Observable<GetAnimalsHealthByIdDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetAnimalsHealthByIdDto>(`${this.apiUrl}/${request}`);
  }
}
