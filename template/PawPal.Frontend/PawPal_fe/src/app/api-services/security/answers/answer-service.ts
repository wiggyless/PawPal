import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { PostAnswerDTO, VerifyAnswerDTO, IsAnswerTrue } from './answer-model';
import { buildHttpParams } from '../../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class SecurityAnswerService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/SecurityAnswers';

  createSecurityAnswer(request: PostAnswerDTO): Observable<number> {
    return this.httpClient.post<number>(this.apiUrl, request);
  }
  checkSecurityAnswer(request: VerifyAnswerDTO): Observable<IsAnswerTrue> {
    return this.httpClient.post<IsAnswerTrue>(`${this.apiUrl}/verify`, request);
  }
  updateSecurityAnswers(request: PostAnswerDTO): Observable<any> {
    return this.httpClient.put<number>(this.apiUrl, request);
  }
}
