import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { ListAnimalBreedQueryDto } from './animal-breed.model';
import { buildHttpParams } from '../../core/models/build-http-params';
import { PageResult } from '../../core/models/paging/page-result';

@Injectable({
  providedIn: 'root',
})
export class AnimalBreedService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Breed';

  listAnimalBreed(request?: any): Observable<PageResult<ListAnimalBreedQueryDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListAnimalBreedQueryDto>>(this.apiUrl, { params });
  }
}
