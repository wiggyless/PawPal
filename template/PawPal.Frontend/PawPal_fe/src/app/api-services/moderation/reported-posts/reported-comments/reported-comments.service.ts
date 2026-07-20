import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { buildHttpParams } from '../../../../core/models/build-http-params';
import { PageResult } from '../../../../core/models/paging/page-result';
import {
  CreateReportedCommentCommand,
  GetReportedCommentDto,
  GetReportedCommentQuery,
} from './reported-comments.model';

@Injectable({ providedIn: 'root' })
export class ReportCommentService {
  http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/ReportedComments';

  constructor() {}

  createCommentReport(command: CreateReportedCommentCommand): Observable<number> {
    return this.http.post<number>(this.apiUrl, command);
  }
  deleteCommentReport(id: number): Observable<any> {
    return this.http.delete<number>(`${this.apiUrl}/${id}`);
  }
  getCommentReportList(
    request: GetReportedCommentQuery,
  ): Observable<PageResult<GetReportedCommentDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<PageResult<GetReportedCommentDto>>(this.apiUrl, { params });
  }
  getCommentReportByID(id: number): Observable<GetReportedCommentDto> {
    const params = id ? buildHttpParams(id as any) : undefined;
    return this.http.get<GetReportedCommentDto>(`${this.apiUrl}/${id}`, { params });
  }
}
