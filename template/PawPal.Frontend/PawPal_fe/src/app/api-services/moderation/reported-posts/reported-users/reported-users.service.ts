import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import {
  CreateReportedUserCommand,
  GetUserReportDto,
  GetUserReportQuery,
} from './reported-users.model';
import { buildHttpParams } from '../../../../core/models/build-http-params';
import { PageResult } from '../../../../core/models/paging/page-result';

@Injectable({ providedIn: 'root' })
export class ReportUserService {
  http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/ReportedUsers';

  constructor() {}

  createUserReport(command: CreateReportedUserCommand): Observable<number> {
    return this.http.post<number>(this.apiUrl, command);
  }
  deleteUserReport(id: number): Observable<any> {
    return this.http.delete<number>(`${this.apiUrl}/${id}`);
  }
  getUserReportList(request: GetUserReportQuery): Observable<PageResult<GetUserReportDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<PageResult<GetUserReportDto>>(this.apiUrl, { params });
  }
  getUserReportByID(id: number): Observable<GetUserReportDto> {
    const params = id ? buildHttpParams(id as any) : undefined;
    return this.http.get<GetUserReportDto>(`${this.apiUrl}/${id}`, { params });
  }
}
