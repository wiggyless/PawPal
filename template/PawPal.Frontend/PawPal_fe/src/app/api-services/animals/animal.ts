import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AnimalService {
  constructor(){}
    private http = inject(HttpClient);
    private apiURL = environment.apiUrl + '/Animals';
  
    public get() : Observable<any>{
      return this.http.get(this.apiURL);
    }
}
