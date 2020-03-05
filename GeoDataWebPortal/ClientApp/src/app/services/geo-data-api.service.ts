import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { GeoDataset } from '../models/geo-dataset';
import { Observable, of, Subject } from 'rxjs';
import { SettingsService } from '../portal-settings/settings.service';
import { AuthService } from '../auth/auth.service';
import { switchMap, delay, repeat, tap, catchError } from 'rxjs/operators';
import { URLSettings } from '../portal-settings/models/urlsettings';

@Injectable({
  providedIn: 'root'
})

export class GeoDataAPIService {

  private readonly API_ENDPOINT: string = 'api/GeoData';
  private readonly API_HEALTH_ENDPOINT: string = 'health';

  constructor(
    private http: HttpClient,
    private settingsService: SettingsService,
    private authService: AuthService) { }

  public Get(id: string): Observable<GeoDataset> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataSingleURL(settings, this.API_ENDPOINT, id);
            return this.http.get<GeoDataset>(url, { headers: httpHeaders });
          }));
      })
    );
  }

  public GetAll(filter?: string): Observable<Array<GeoDataset>> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataURL(settings, this.API_ENDPOINT);
            const httpPrms = new HttpParams();
            httpPrms.append('filter', filter);
            return this.http.get<Array<GeoDataset>>(url, { headers: httpHeaders, params: httpPrms });
          }));
      })
    );
  }

  public create(data: GeoDataset): Observable<any> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataURL(settings, this.API_ENDPOINT);
            return this.http.post<GeoDataset>(url, data, { headers: httpHeaders });
          }));
      })
    );
  }

  public update(data: GeoDataset) {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataURL(settings, this.API_ENDPOINT) + '/' + data.ID;
            console.log("put:", data, " url:", url, "headers:", Headers)
            return this.http.put<GeoDataset>(url, data, { headers: httpHeaders });
          }));
      })
    );
  }

  public delete(id: string): Observable<GeoDataset> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataSingleURL(settings, this.API_ENDPOINT, id);
            return this.http.delete<GeoDataset>(url, { headers: httpHeaders });
          }));
      })
    );
  }

  private getSettingsObservable(): Observable<URLSettings> {
    return this.settingsService.getURLSettings();
  }

  private getHttpHeaders(): Observable<HttpHeaders> {
    return this.authService.token.pipe(switchMap(beartoken => {
      return of(this.getHeaderOptions(beartoken));
    }));
  }

  private getGeoDataSingleURL(urlSettings: URLSettings, endPoint: string, id: string): string {
    return urlSettings.GeoManagerAPI + '/' + endPoint + '/' + id;
  }

  private getGeoDataURL(urlSettings: URLSettings, endPoint: string): string {
    return urlSettings.GeoManagerAPI + '/' + endPoint;
  }

  private getHeaderOptions(token: string): HttpHeaders {
    var httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8',
      'Authorization': 'Bearer ' + token
    });
    return httpHeaders;
  }

  private getApiHealth(): Observable<any> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        const url = this.getGeoDataURL(settings, this.API_HEALTH_ENDPOINT);
        return this.http.get(url, {responseType: 'text'});
      })
    );
  }

  public apiHealthCheckPolling(): Observable<any> {
    return this.getApiHealth()    
    .pipe(
      catchError(err=>{
        console.log(err);
        return of('UnHealthy');
      }),
      delay(5000), 
      repeat());
  }

}