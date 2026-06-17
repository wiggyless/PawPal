import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { GetAndPostAnswerDTO, IsAnswerTrue } from './answer-model';
import { buildHttpParams } from '../../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class SecurityAnswerService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/SecurityAnswers';

  createSecurityAnswer(request: GetAndPostAnswerDTO): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<number>(this.apiUrl, request, { params });
  }
  checkSecurityAnswer(request: GetAndPostAnswerDTO): Observable<IsAnswerTrue> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<IsAnswerTrue>(this.apiUrl, { params });
  }
}
