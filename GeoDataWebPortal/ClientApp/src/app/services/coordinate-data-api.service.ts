import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { SettingsService } from '../portal-settings/settings.service';
import { AuthService } from '../auth/auth.service';
import { switchMap } from 'rxjs/operators';
import { ConfigurationSettings } from '../portal-settings/models/urlsettings';
import { CoordinateData } from '../models/coordinate-data';

@Injectable({
    providedIn: 'root'
})

export class CoordinateDataAPIService {

    private readonly API_ENDPOINT: string = 'api/CoordinateData';
    private readonly API_ENDPOINT_SHARED: string = 'api/CoordinateData/shared';
        
    constructor(
        private http: HttpClient,
        private settingsService: SettingsService,
        private authService: AuthService) { }

    public Get(id: string): Observable<CoordinateData> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getCoordinateDataSingleURL(settings, this.API_ENDPOINT, id);
                        return this.http.get<CoordinateData>(url, { headers: httpHeaders });
                    }));
            })
        );
    }

    public GetAllOwned(filter?: Map<string, string>): Observable<Array<CoordinateData>> {
        return this.GetAll(this.API_ENDPOINT, filter);
    }

    public GetAllShared(filter?: Map<string, string>): Observable<Array<CoordinateData>> {
        return this.GetAll(this.API_ENDPOINT_SHARED, filter);
    }

    public GetAll(apiUrl: string, filter?: Map<string, string>): Observable<Array<CoordinateData>> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getFullURL(settings, apiUrl);
                        const httpPrms = this.getFilterQueryStringPrms(filter);
                        return this.http.get<Array<CoordinateData>>(url, { headers: httpHeaders, params: httpPrms });
                    }));
            })
        );
    }

    public create(data: CoordinateData): Observable<any> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getFullURL(settings, this.API_ENDPOINT);
                        return this.http.post<CoordinateData>(url, data, { headers: httpHeaders });
                    }));
            })
        );
    }

    public update(data: CoordinateData) {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getFullURL(settings, this.API_ENDPOINT) + '/' + data.ID;
                        console.log("put:", data, " url:", url, "headers:", Headers)
                        return this.http.put<CoordinateData>(url, data, { headers: httpHeaders });
                    }));
            })
        );
    }

    public delete(id: string): Observable<CoordinateData> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getCoordinateDataSingleURL(settings, this.API_ENDPOINT, id);
                        return this.http.delete<CoordinateData>(url, { headers: httpHeaders });
                    }));
            })
        );
    }

    private getSettingsObservable(): Observable<ConfigurationSettings> {
        return this.settingsService.getSettings();
    }

    private getHttpHeaders(): Observable<HttpHeaders> {
        return this.authService.token.pipe(switchMap(beartoken => {
            return of(this.getHeaderOptions(beartoken));
        }));
    }

    private getCoordinateDataSingleURL(urlSettings: ConfigurationSettings, endPoint: string, id: string): string {
        return urlSettings.GeoDataApiUrl + '/' + endPoint + '/' + id;
    }

    private getFullURL(urlSettings: ConfigurationSettings, endPoint: string): string {
        return urlSettings.GeoDataApiUrl + '/' + endPoint;
    }

    private getHeaderOptions(token: string): HttpHeaders {
        var httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
            'Authorization': 'Bearer ' + token
        });
        return httpHeaders;
    }

    private getFilterQueryStringPrms(filter: Map<string, string>): HttpParams {
        let httpPrms = new HttpParams();
        if (filter != null) {
            filter.forEach((val, key) => {
                httpPrms = httpPrms.append(key, val);
            });
        }
        return httpPrms;
    }
}