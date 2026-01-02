import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { ListAnimalPostsDto } from './animal-posts.model';
@Injectable({
  providedIn: 'root',
})
export class AnimalPostService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Posts';

  listAnimalPosts(request?: any): Observable<ListAnimalPostsDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<ListAnimalPostsDto>(this.apiUrl, { params });
  }
}
