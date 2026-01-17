import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';
import { ListDisabilitiesQueryDto } from './disability-model';

@Injectable({
  providedIn: 'root',
})
export class DisabilityService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Disabilities';

  listAnimalDisability(request?: any): Observable<ListDisabilitiesQueryDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<ListDisabilitiesQueryDto>(this.apiUrl, { params });
  }
}
