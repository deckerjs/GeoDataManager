import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of} from 'rxjs';
import { SettingsService } from '../portal-settings/settings.service';
import { switchMap, delay, repeat, catchError } from 'rxjs/operators';
import { ConfigurationSettings } from '../portal-settings/models/urlsettings';

@Injectable({
    providedIn: 'root'
})

export class HealthCheckAPIService {
    private readonly API_HEALTH_ENDPOINT: string = 'health';

    constructor(
        private http: HttpClient,
        private settingsService: SettingsService) { }

    private getSettingsObservable(): Observable<ConfigurationSettings> {
        return this.settingsService.getSettings();
    }

    private getFullURL(urlSettings: ConfigurationSettings, endPoint: string): string {
        return urlSettings.GeoDataApiUrl + '/' + endPoint;
    }

    private getApiHealth(): Observable<any> {
        return this.getSettingsObservable().pipe(
            switchMap(settings => {
                const url = this.getFullURL(settings, this.API_HEALTH_ENDPOINT);
                return this.http.get(url, { responseType: 'text' });
            })
        );
    }

    public apiHealthCheckPolling(): Observable<any> {
        return this.getApiHealth()
            .pipe(
                catchError(err => {
                    console.log(err);
                    return of('UnHealthy');
                }),
                delay(5000),
                repeat());
    }

}