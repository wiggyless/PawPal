import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { GetPostImageById } from './animal-post-images-model';
@Injectable({
  providedIn: 'root',
})
export class PostImagesService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/PostImages';

  getImagePost(request?: any): Observable<GetPostImageById> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetPostImageById>(`${this.apiUrl}/${request}`);
  }
}
