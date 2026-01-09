import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { finalize, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GetCityByIdDto, ListCitiesQueryDto } from './cities.model';
import { buildHttpParams } from '../../core/models/build-http-params';
@Injectable({
  providedIn: 'root',
})
export class CitiesService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Cities';
  listCities(request?: any): Observable<ListCitiesQueryDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<ListCitiesQueryDto>(this.apiUrl, { params });
  }
  getCityById(request?: number): Observable<GetCityByIdDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetCityByIdDto>(`${this.apiUrl}/${request}`);
  }
}
