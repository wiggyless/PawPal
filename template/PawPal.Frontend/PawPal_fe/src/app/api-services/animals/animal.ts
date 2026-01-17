import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';
import { AddAnimalDto, GetAnimalByIdDto } from './animal-model';

@Injectable({
  providedIn: 'root',
})
export class AnimalService {
  private http = inject(HttpClient);
  private apiURL = environment.apiUrl + '/Animals';

  getAnimalById(request?: any): Observable<GetAnimalByIdDto> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<GetAnimalByIdDto>(`${this.apiURL}/${request}`, { params });
  }
  addAnimal(request?: any): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.post<number>(`${this.apiURL}`, request, { params });
  }
  updateAnimal(request: any, id: number): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.put<number>(`${this.apiURL}/${id}`, request, { params });
  }
}
