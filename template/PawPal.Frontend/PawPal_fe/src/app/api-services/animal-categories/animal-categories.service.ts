import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ListAnimalCategoriesQueryDto } from './animal-categories.model';
import { buildHttpParams } from '../../core/models/build-http-params';
@Injectable({
  providedIn: 'root',
})

export class AnimalCategoriesService {
    httpClient = inject(HttpClient);
    private apiUrl = environment.apiUrl + '/AnimalCategories';

    listAnimalCategories(request? : any): Observable<ListAnimalCategoriesQueryDto> {
            const params = request ? buildHttpParams(request as any) : undefined;
            return this.httpClient.get<ListAnimalCategoriesQueryDto>(this.apiUrl, { params });
    }

}