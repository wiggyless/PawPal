import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';
import { PageResult } from '../../core/models/paging/page-result';
import {
  GetAdoptionRequestListQuery,
  GetAdoptionRequestList,
  GetAdoptionRequestById,
  CreateAdoptionRequest,
  GetAdoptionRequestListByPostIDQuery,
  GetAdoptionRequestByPostID,
  UpdateRequestByID,
} from './animals-adoption-model';
import { CreateAdoptionRequirement } from '../animals-requirements/animals-requirements-model';

@Injectable({
  providedIn: 'root',
})
export class AnimalRequestService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/AdoptionRequest';
  listAnimalRequests(
    request?: GetAdoptionRequestListQuery,
  ): Observable<PageResult<GetAdoptionRequestList>> {
    const params = request ? buildHttpParams(request as any) : undefined;

    return this.httpClient.get<PageResult<GetAdoptionRequestList>>(this.apiUrl, { params });
  }

  listAnimalRequestsHistory(
    request?: GetAdoptionRequestListQuery,
  ): Observable<PageResult<GetAdoptionRequestList>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<GetAdoptionRequestList>>(this.apiUrl + '/history', {
      params,
    });
  }
  getAnimalRequestById(request: number): Observable<GetAdoptionRequestById> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetAdoptionRequestById>(`${this.apiUrl}/${request}`, { params });
  }

  addRequest(request?: CreateAdoptionRequest): Observable<number> {
    return this.httpClient.post<number>(`${this.apiUrl}`, request);
  }

  addRequestWithRequirement(request: CreateAdoptionRequirement): Observable<{ id: number }> {
    return this.httpClient.post<{ id: number }>(`${this.apiUrl}/with-requirement`, request);
  }

  deleteRequest(requestId: number): Observable<number> {
    return this.httpClient.delete<number>(`${this.apiUrl}/${requestId}`, {
      body: {
        requestID: requestId,
      },
    });
  }

  listByPostID(
    request?: GetAdoptionRequestListByPostIDQuery,
  ): Observable<PageResult<GetAdoptionRequestByPostID>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<GetAdoptionRequestByPostID>>(`${this.apiUrl}/userPost`, {
      params,
    });
  }

  updateRequest(request: UpdateRequestByID): Observable<void> {
    return this.httpClient.put<void>(`${this.apiUrl}/${request.requestID}/status`, request);
  }
}
