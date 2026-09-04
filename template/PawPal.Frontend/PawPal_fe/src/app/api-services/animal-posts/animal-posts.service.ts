import { createEnvironmentInjector, EnvironmentInjector, inject, Injectable } from '@angular/core';
import {
  HttpClient,
  HttpEvent,
  HttpParams,
  provideHttpClient,
  withInterceptors,
} from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { authInterceptor } from '../../core/interceptors/auth-interceptor.service';
import { rateLimitInterceptor } from '../../core/interceptors/rate-limit-interceptor.service';
import {
  AddAnimalPost,
  AnimalPostByIdQuery,
  GetListPostQueryByUserID,
  GetPostQuery,
  ListAnimal,
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

  private uploadHttpClient = createEnvironmentInjector(
    [provideHttpClient(withInterceptors([authInterceptor, rateLimitInterceptor]))],
    inject(EnvironmentInjector),
  ).get(HttpClient);

  listAnimalPosts(request?: GetPostQuery): Observable<PageResult<ListAnimal>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListAnimal>>(this.apiUrl, { params });
  }
  listAnimalPostsByUserId(request: GetListPostQueryByUserID): Observable<PageResult<ListAnimal>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListAnimal>>(`${this.apiUrl}/userPost`, {
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
  addAnimalPost(formData: FormData): Observable<HttpEvent<ResponseImage>> {
    return this.uploadHttpClient.post<ResponseImage>(`${this.apiUrl}/animal`, formData, {
      reportProgress: true,
      observe: 'events',
    });
  }
  updateAnimalPost(id: number, formData: FormData): Observable<HttpEvent<void>> {
    return this.uploadHttpClient.put<void>(`${this.apiUrl}/animal/${id}`, formData, {
      reportProgress: true,
      observe: 'events',
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
