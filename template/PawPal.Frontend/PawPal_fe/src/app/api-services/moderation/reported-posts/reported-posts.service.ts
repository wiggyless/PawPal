import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateReportedPostCommand } from './reported-posts.model'
import { environment } from '../../../../environments/environment.development';


@Injectable({ providedIn: 'root' })
export class ReportedPostsService {
   http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/ReportedPosts';

  constructor() {}

  createReport(command: CreateReportedPostCommand): Observable<any> {
    return this.http.post(this.apiUrl, command);
  }
}