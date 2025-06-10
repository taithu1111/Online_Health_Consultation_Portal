// specialization.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserEnviroment } from 'environments/environment';

@Injectable({
    providedIn: 'root'
})
export class SpecializationService {
    private apiUrl = `${UserEnviroment.apiUrl}/specializations`;

    constructor(private http: HttpClient) { }

    getAllSpecializations(): Observable<any[]> {
        // console.log('Making request to:', this.apiUrl); // Debug log
        return this.http.get<any[]>(`${this.apiUrl}`);
    }

    searchSpecializations(term: string): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/search`, {
            params: { term }
        });
    }
}