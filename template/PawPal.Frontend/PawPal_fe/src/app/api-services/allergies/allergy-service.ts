import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { ListAllergyQueryDto } from './allergy-model';
import { PageResult } from '../../core/models/paging/page-result';
@Injectable({
  providedIn: 'root',
})
export class AllergyService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Allergies';

  listAnimalAllergies(request?: any): Observable<PageResult<ListAllergyQueryDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListAllergyQueryDto>>(this.apiUrl, { params });
  }
  getAnimalArrergieByUserID(request?: any): Observable<ListAllergyQueryDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<ListAllergyQueryDto>(`${this.apiUrl}/${request}`, { params });
  }
}
