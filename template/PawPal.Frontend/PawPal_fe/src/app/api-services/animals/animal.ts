import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { buildHttpParams } from '../../core/models/build-http-params';
import { GetAnimalByIdDto } from './animal-model';

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
}
