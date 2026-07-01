import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { buildHttpParams } from '../../../core/models/build-http-params';
import { PageResult } from '../../../core/models/paging/page-result';
import {
  CreateReportedProblem,
  ReportedProblemsDto,
  ReportedProblemsQuery,
} from './reported-problem.model';

@Injectable({ providedIn: 'root' })
export class ReportedProblemService {
  http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/ReportedProblems';

  constructor() {}

  createProblemReport(command: CreateReportedProblem): Observable<any> {
    return this.http.post(this.apiUrl, command);
  }
  getProblemReportList(
    request: ReportedProblemsQuery,
  ): Observable<PageResult<ReportedProblemsDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<PageResult<ReportedProblemsDto>>(this.apiUrl, { params });
  }
  getProblemReportByID(id: number): Observable<ReportedProblemsDto> {
    const params = id ? buildHttpParams(id as any) : undefined;
    return this.http.get<ReportedProblemsDto>(`${this.apiUrl}/${id}`, { params });
  }
  deleteProblemReport(id: number): Observable<any> {
    return this.http.delete<number>(`${this.apiUrl}/${id}`);
  }
}
