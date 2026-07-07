import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';
import { ListDisabilitiesQueryDto } from './disability-model';
import { PageResult } from '../../core/models/paging/page-result';

@Injectable({
  providedIn: 'root',
})
export class DisabilityService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Disabilities';

  listAnimalDisability(request?: any): Observable<PageResult<ListDisabilitiesQueryDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListDisabilitiesQueryDto>>(this.apiUrl, { params });
  }
}
