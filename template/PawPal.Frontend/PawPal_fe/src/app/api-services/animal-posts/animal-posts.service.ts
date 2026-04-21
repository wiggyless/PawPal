import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import {
  AddAnimalPost,
  AnimalPostByIdQuery,
  GetPostQuery,
  ListAnimal,
  listAnimalPostsByUserIdDto,
  ListPostsByRange,
} from './animal-posts.model';
import { PageResult } from '../../core/models/paging/page-result';
interface ResponseImage {
  id: number;
}
@Injectable({
  providedIn: 'root',
})
export class AnimalPostService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Posts';

  listAnimalPosts(request?: any): Observable<PageResult<ListAnimal>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListAnimal>>(this.apiUrl, { params });
  }
  listAnimalPostsByUserId(request: any): Observable<PageResult<listAnimalPostsByUserIdDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    console.log(params?.toString());
    return this.httpClient.get<PageResult<listAnimalPostsByUserIdDto>>(`${this.apiUrl}/userPost`, {
      params,
    });
  }
  getPostById(request?: any): Observable<AnimalPostByIdQuery> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<AnimalPostByIdQuery>(`${this.apiUrl}/${request}`, { params });
  }
  addPost(request?: any): Observable<ResponseImage> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<ResponseImage>(`${this.apiUrl}`, request, { params });
  }
  listPostRange(request: ListPostsByRange): Observable<PageResult<listAnimalPostsByUserIdDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    console.log(request);
    return this.httpClient.get<PageResult<listAnimalPostsByUserIdDto>>(`${this.apiUrl}/likedPost`, {
      params,
    });
  }
  deletePost(postId: number, animalId: number): Observable<number> {
    return this.httpClient.delete<number>(`${this.apiUrl}/${postId}`, {
      body: {
        id: postId,
        animalID: animalId,
      },
    });
  }
}
