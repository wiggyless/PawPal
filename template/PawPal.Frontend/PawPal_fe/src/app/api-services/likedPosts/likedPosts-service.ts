import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';

import { PageResult } from '../../core/models/paging/page-result';
import {
  DeleteLikedPost,
  GetLikedPostList,
  GetLikedPostListQuery,
  LikePost,
} from './likedPosts-model';
interface ResponseImage {
  id: number;
}
@Injectable({
  providedIn: 'root',
})
export class LikedPostsService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/LikedPosts';

  listLikedPosts(request: GetLikedPostListQuery): Observable<GetLikedPostList> {
    const params = request ? buildHttpParams(request as any) : undefined;
    console.log(params?.toString());
    return this.httpClient.get<GetLikedPostList>(this.apiUrl, { params });
  }
  addLikedPosts(request: LikePost): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<number>(`${this.apiUrl}`, request, { params });
  }
  deletePost(request: DeleteLikedPost): Observable<number> {
    return this.httpClient.delete<number>(`${this.apiUrl}/${request.userId}`, {
      body: {
        userId: request.userId,
        postId: request.postId,
      },
    });
  }
}
