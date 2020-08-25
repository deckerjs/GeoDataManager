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

export class ImageImportDataService {

    private readonly API_IMAGE_ENDPOINT: string = 'api/imageupload';

    constructor(
        private http: HttpClient,
        private settingsService: SettingsService,
        private authService: AuthService) { }


    public imageUploadToNew(data: string): Observable<any> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getFullURL(settings, this.API_IMAGE_ENDPOINT);
                        return this.http.post(url, { ImageData: data }, { headers: httpHeaders });
                    }));
            })
        );
    }

    public imageUploadToExisting(selectedImportId: string, data: string): Observable<any> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                return this.getHttpHeaders().pipe(
                    switchMap(httpHeaders => {
                        const url = this.getFullURL(settings, this.API_IMAGE_ENDPOINT) + '/' + selectedImportId;
                        return this.http.post(url, { ImageData: data }, { headers: httpHeaders });
                    }));
            })
        );
    }

    private getSettingsObservable(): Observable<ConfigurationSettings> {
        return this.settingsService.getSettings();
    }

    private getHttpHeaders(): Observable<HttpHeaders> {
        return this.authService.token.pipe(switchMap(beartoken => {
            return of(new HttpHeaders({
                'Content-Type': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + beartoken
            }));
        }));
    }

    private getFullURL(urlSettings: ConfigurationSettings, endPoint: string): string {
        return urlSettings.GeoDataApiUrl + '/' + endPoint;
    }

}