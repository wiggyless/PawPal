import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, map, Observable, of, switchMap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { AddNewPostImages, GetImagePostBlob, GetPostImageById } from './animal-post-images-model';
@Injectable({
  providedIn: 'root',
})
export class PostImagesService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/PostImages';

  getImagePost(request?: any): Observable<string[]> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient
      .get<GetPostImageById>(`${this.apiUrl}/${request}`)
      .pipe(map((x) => x.postImages));
  }
  getImagePostBlob(request?: any): Observable<GetImagePostBlob> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.httpClient.get<GetImagePostBlob>(`${this.apiUrl}/download/${request}`);
  }
  updatePostImages(request: AddNewPostImages): Observable<number> {
    const params = request ? buildHttpParams(request as any) : undefined;
    const formData = new FormData();
    formData.append('postId', request.postId.toString());
    console.log(request.postImages);
    request.postImages.forEach((x) => {
      formData.append('postImages', x, x.name);
    });
    return this.httpClient.put<number>(`${this.apiUrl}`, formData);
  }
}
