import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { SettingsService } from '../portal-settings/settings.service';
import { AuthService } from '../auth/auth.service';
import { switchMap } from 'rxjs/operators';
import { ConfigurationSettings } from '../portal-settings/models/urlsettings';

@Injectable({
    providedIn: 'root'
})

export class GpxImportDataService {

    private readonly API_GPX_ENDPOINT: string = 'api/gpxupload';

    constructor(
        private http: HttpClient,
        private settingsService: SettingsService,
        private authService: AuthService) { }


    public gpxUpload(data: string): Observable<any> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getXMLHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getFullURL(settings, this.API_GPX_ENDPOINT);
                        return this.http.post(url, data, { headers: httpHeaders });
                    }));
            })
        );
    }

    private getSettingsObservable(): Observable<ConfigurationSettings> {
        return this.settingsService.getSettings();
    }

    private getXMLHttpHeaders(): Observable<HttpHeaders> {
        return this.authService.token.pipe(switchMap(beartoken => {
            return of(new HttpHeaders({
                'Content-Type': 'application/xml; charset=utf-8',
                'Authorization': 'Bearer ' + beartoken
            }));
        }));
    }

    private getFullURL(urlSettings: ConfigurationSettings, endPoint: string): string {
        return urlSettings.GeoDataApiUrl + '/' + endPoint;
    }

}