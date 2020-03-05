import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, of, Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class ConfigurationSettingsService {

    private readonly API_ENDPOINT: string = '/api/ConfigurationSettings';

    constructor(
        private http: HttpClient
    ) {
    }


    public GetSettings(id: string): Observable<ConfigurationSettings> {
        return this.http.get<ConfigurationSettings>(this.API_ENDPOINT);
    }

}

export class ConfigurationSettings {
    public AuthUrl: string;
    public AuthClientSecret: string;
    public GeoDataApiUrl: string;
    public MapboxToken: string;
}