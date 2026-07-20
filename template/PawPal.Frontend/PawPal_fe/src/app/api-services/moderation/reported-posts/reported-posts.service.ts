import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  CreateReportedPostCommand,
  GetReportedPostDto,
  GetReportedPostQuery,
} from './reported-posts.model';
import { environment } from '../../../../environments/environment';
import { buildHttpParams } from '../../../core/models/build-http-params';
import { PageResult } from '../../../core/models/paging/page-result';

@Injectable({ providedIn: 'root' })
export class ReportedPostsService {
  http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/ReportedPosts';

  constructor() {}

  createReport(command: CreateReportedPostCommand): Observable<any> {
    return this.http.post(this.apiUrl, command);
  }
  getPostReportList(request: GetReportedPostQuery): Observable<PageResult<GetReportedPostDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<PageResult<GetReportedPostDto>>(this.apiUrl, { params });
  }
  getPostReportByID(id: number): Observable<GetReportedPostDto> {
    const params = id ? buildHttpParams(id as any) : undefined;
    return this.http.get<GetReportedPostDto>(`${this.apiUrl}/${id}`, { params });
  }
  deletePostReport(id: number): Observable<any> {
    return this.http.delete<number>(`${this.apiUrl}/${id}`);
  }
}
