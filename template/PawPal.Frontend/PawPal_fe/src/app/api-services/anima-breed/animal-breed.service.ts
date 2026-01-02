import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { ListAnimalBreedQueryDto } from './animal-breed.model';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class AnimalBreedService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Breed';

  listAnimalBreed(request?: any): Observable<ListAnimalBreedQueryDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<ListAnimalBreedQueryDto>(this.apiUrl, { params });
  }
}
