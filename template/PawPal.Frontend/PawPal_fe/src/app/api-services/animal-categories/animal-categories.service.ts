import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AnimalCategoryByIdQueryDto,
  ListAnimalCategoriesQueryDto,
} from './animal-categories.model';
import { buildHttpParams } from '../../core/models/build-http-params';
import { PageResult } from '../../core/models/paging/page-result';
@Injectable({
  providedIn: 'root',
})
export class AnimalCategoriesService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/AnimalCategories';

  listAnimalCategories(request?: any): Observable<PageResult<ListAnimalCategoriesQueryDto>> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<PageResult<ListAnimalCategoriesQueryDto>>(this.apiUrl, { params });
  }

  getAnimalCategoryById(request?: number): Observable<AnimalCategoryByIdQueryDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<AnimalCategoryByIdQueryDto>(`${this.apiUrl}/${request}`);
  }
}
