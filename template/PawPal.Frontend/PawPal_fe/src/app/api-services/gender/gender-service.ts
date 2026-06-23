import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { ListGenderDto } from './gender-model';
import { PageResult } from '../../core/models/paging/page-result';
@Injectable({
  providedIn: 'root',
})
export class GenderService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Gender';

  listGender(request?: any): Observable<PageResult<ListGenderDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListGenderDto>>(this.apiUrl, { params });
  }
}
