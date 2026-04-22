import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';
import { PageResult } from '../../core/models/paging/page-result';
import {
  GetAdoptionRequirementQuery,
  GetAdoptionRequirementList,
  GetAdoptionRequirementsById,
  CreateAdoptionRequirement
} from './animals-requirements-model';
@Injectable({
  providedIn: 'root'
})
export class AnimalRequirementService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/AdotptionRequirements';
  listAnimalRequirements(
    request?: GetAdoptionRequirementQuery
  ): Observable<PageResult<GetAdoptionRequirementList>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    console.log(params?.toString());
    return this.httpClient.get<PageResult<GetAdoptionRequirementList>>(this.apiUrl, { params });
  }
  getAnimalRequirementsById(request: number): Observable<GetAdoptionRequirementsById> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetAdoptionRequirementsById>(`${this.apiUrl}/${request}`, {
      params
    });
  }
  addRequirements(request?: CreateAdoptionRequirement): Observable<any> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.post<any>(`${this.apiUrl}`, request, { params });
  }
  deleteRequirements(requestId: number): Observable<number> {
    return this.httpClient.delete<number>(`${this.apiUrl}/${requestId}`, {
      body: {
        requestID: requestId
      }
    });
  }
}
