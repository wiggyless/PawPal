import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { ListCantonsDto } from './cantons-model';
import { PageResult } from '../../core/models/paging/page-result';
@Injectable({
  providedIn: 'root',
})
export class CantonsService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Cantons';

  listCantons(request?: any): Observable<PageResult<ListCantonsDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListCantonsDto>>(this.apiUrl, { params });
  }
  getCantonById(request?: any): Observable<ListCantonsDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<ListCantonsDto>(`${this.apiUrl}/${request}`);
  }
}
