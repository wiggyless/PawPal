import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { PageResult } from '../../core/models/paging/page-result';
import { CreateNewsRequest, GetNewsByIdQueryDto, ListNewsQuery, ListNewsQueryDto } from './news.model';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/News';

  listNews(request?: ListNewsQuery): Observable<PageResult<ListNewsQueryDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListNewsQueryDto>>(this.apiUrl, { params });
  }

  getNewsById(id: number): Observable<GetNewsByIdQueryDto> {
    return this.httpClient.get<GetNewsByIdQueryDto>(`${this.apiUrl}/${id}`);
  }

  createNews(request: CreateNewsRequest): Observable<{ id: number }> {
    const formData = new FormData();
    formData.append('title', request.title);
    formData.append('content', request.content);
    formData.append('photo', request.photo, request.photo.name);
    return this.httpClient.post<{ id: number }>(this.apiUrl, formData);
  }
}
