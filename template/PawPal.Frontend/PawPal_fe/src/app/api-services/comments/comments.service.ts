import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';
import { PageResult } from '../../core/models/paging/page-result';
import { CommentDto, CommentQuery, CreateCommentCommand } from './comments.model';
@Injectable({
  providedIn: 'root',
})
export class CommentService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Comments';
  getComments(request?: CommentQuery): Observable<PageResult<CommentDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<CommentDto>>(this.apiUrl, {
      params,
    });
  }
  postComment(request?: CreateCommentCommand): Observable<number> {
    console.log(request);
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<number>(`${this.apiUrl}/post`, request, { params });
  }
}
