import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { GetUserImageById, UserImageCommand, UserImageDto, UserImageQuery } from './userImage-model';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
@Injectable({
  providedIn: 'root',
})
export class UserImageService {
  httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/UserImage';
  private imageCache = new Map<number, SafeUrl>();
  private sanitizer = inject(DomSanitizer);

 getUserImageByID(userID: number): Observable<SafeUrl> {
  if (this.imageCache.has(userID)) {
    return of(this.imageCache.get(userID)!);
  }

  return this.httpClient.get<GetUserImageById>(`${this.apiUrl}/${userID}`).pipe(
    map((photo) => {
      const fullUrl = photo.photoURL.startsWith('http')
        ? photo.photoURL
        : `${environment.apiUrl}${photo.photoURL}`;
      const safeUrl = this.sanitizer.bypassSecurityTrustUrl(fullUrl);
      this.imageCache.set(userID, safeUrl);
      return safeUrl;
    }),
  );
}
createUserImage(userID: number, image: File): Observable<number> {
  const formData = new FormData();
  formData.append('userID', userID.toString());
  formData.append('Image', image, image.name);
  return this.httpClient.post<number>(this.apiUrl, formData);
}

updateUserImage(userID: number, image: File): Observable<number> {
  const formData = new FormData();
  formData.append('userID', userID.toString());
  formData.append('Image', image, image.name);
  return this.httpClient.put<number>(this.apiUrl, formData);
}
}
