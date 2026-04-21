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
    console.log(params?.toString());
    return this.httpClient.get<PageResult<GetAdoptionRequestList>>(this.apiUrl, { params });
  }
  listAnimalRequestsHistory(
    request?: GetAdoptionRequestListQuery,
  ): Observable<PageResult<GetAdoptionRequestList>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    console.log(params?.toString());
    return this.httpClient.get<PageResult<GetAdoptionRequestList>>(this.apiUrl + '/history', {
      params,
    });
  }
  getAnimalRequestById(request: number): Observable<GetAdoptionRequestById> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetAdoptionRequestById>(`${this.apiUrl}/${request}`, { params });
  }
  addRequest(request?: CreateAdoptionRequest): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<number>(`${this.apiUrl}`, request, { params });
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
    console.log(params?.toString());
    return this.httpClient.get<PageResult<GetAdoptionRequestByPostID>>(`${this.apiUrl}/userPost`, {
      params,
    });
  }
  updateRequest(request: UpdateRequestByID): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.put<number>(`${this.apiUrl}/${request.requestID}`, request, { params });
  }
}
