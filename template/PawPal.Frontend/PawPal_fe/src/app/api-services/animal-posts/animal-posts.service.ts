import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import {
  AddAnimalPost,
  AnimalPostByIdQuery,
  ListAnimal,
  listAnimalPostsByUserIdDto,
} from './animal-posts.model';
import { PageResult } from '../../core/models/paging/page-result';
@Injectable({
  providedIn: 'root',
})
export class AnimalPostService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Posts';

  listAnimalPosts(request?: any): Observable<PageResult<ListAnimal>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    console.log(params?.toString());
    return this.httpClient.get<PageResult<ListAnimal>>(this.apiUrl, { params });
    //.pipe(map((response) => response.items));
  }
  listAnimalPostsByUserId(request?: any): Observable<listAnimalPostsByUserIdDto[]> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient
      .get<PageResult<listAnimalPostsByUserIdDto>>(`${this.apiUrl}/userPost`, { params })
      .pipe(map((response) => response.items));
  }
  getPostById(request?: any): Observable<AnimalPostByIdQuery> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<AnimalPostByIdQuery>(`${this.apiUrl}/${request}`, { params });
  }
  addPost(request?: any): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<number>(`${this.apiUrl}`, request, { params });
  }
}
