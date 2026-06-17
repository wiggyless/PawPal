import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { buildHttpParams } from '../../../core/models/build-http-params';
import {
  GetSecurityQuestionDTO,
  GetSecurityQuestionsQuery,
  GetSecurityQuestionsQueryByEmail,
  PostSecurityQuestion,
} from './questions-model';
import { PageResult } from '../../../core/models/paging/page-result';

@Injectable({
  providedIn: 'root',
})
export class SecurityQuestionService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/SecurityQuestions';

  createSecurityQuestion(request: PostSecurityQuestion): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<number>(this.apiUrl, request, { params });
  }
  getSecurityQuestions(
    request?: GetSecurityQuestionsQuery,
  ): Observable<PageResult<GetSecurityQuestionDTO>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<GetSecurityQuestionDTO>>(this.apiUrl, { params });
  }
  getSecurityQuestionsByEmail(
    request?: GetSecurityQuestionsQueryByEmail,
  ): Observable<PageResult<GetSecurityQuestionDTO>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<GetSecurityQuestionDTO>>(`${this.apiUrl}/email`, {
      params,
    });
  }
}
