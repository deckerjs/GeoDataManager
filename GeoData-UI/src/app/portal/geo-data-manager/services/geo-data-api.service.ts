import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { GeoData } from '../models/geo-data';
import { SettingsService } from '../../portal-settings/settings.service';
import { tap, map, switchMap } from 'rxjs/operators';
import { AuthService } from '../../auth/auth.service';
import { URLSettings } from '../../portal-settings/models/urlsettings';

@Injectable({
  providedIn: 'root'
})
export class GeoDataAPIService {

  private readonly API_ENDPOINT: string = 'api/GeoData';
  constructor(
    private http: HttpClient,
    private settingsService: SettingsService,
    private authService: AuthService) { }

  public Get(id: string): Observable<GeoData> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataSingleURL(settings, this.API_ENDPOINT, id);
            return this.http.get<GeoData>(url, { headers: httpHeaders });
          }));
      })
    );
  }

  public GetAll(filter?: string): Observable<Array<GeoData>> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataURL(settings, this.API_ENDPOINT);
            const httpPrms = new HttpParams();
            httpPrms.append('filter', filter);
            return this.http.get<Array<GeoData>>(url, { headers: httpHeaders, params: httpPrms });
          }));
      })
    );
  }

  public create(data: GeoData):Observable<any> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataURL(settings, this.API_ENDPOINT);
            return this.http.post<GeoData>(url, data, { headers: httpHeaders });
          }));
      })
    );
  }

  public update(data: GeoData) {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataURL(settings, this.API_ENDPOINT);
            return this.http.post<GeoData>(url, data, { headers: httpHeaders });
          }));
      })
    );
  }

  public delete(id: string): Observable<GeoData> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataSingleURL(settings, this.API_ENDPOINT, id);
            return this.http.delete<GeoData>(url, { headers: httpHeaders });
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

}
