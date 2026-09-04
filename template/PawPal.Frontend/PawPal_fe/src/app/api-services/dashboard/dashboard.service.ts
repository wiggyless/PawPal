import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GetDashboardSummaryDto } from './dashboard.model';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Dashboard';

  getSummary(): Observable<GetDashboardSummaryDto> {
    return this.http.get<GetDashboardSummaryDto>(`${this.apiUrl}/summary`);
  }
}
