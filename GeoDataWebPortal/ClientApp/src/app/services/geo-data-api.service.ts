import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { GeoDataset } from '../models/geo-dataset';
import { Observable, of, Subject } from 'rxjs';
import { SettingsService } from '../portal-settings/settings.service';
import { AuthService } from '../auth/auth.service';
import { switchMap, delay, repeat, tap, catchError } from 'rxjs/operators';
import { ConfigurationSettings } from '../portal-settings/models/urlsettings';

@Injectable({
  providedIn: 'root'
})

export class GeoDataAPIService {

  private readonly API_ENDPOINT: string = 'api/GeoData';
  private readonly API_ENDPOINT_SHARED: string = 'api/GeoData/shared';

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

  public GetAllOwned(filter?: Map<string, string>): Observable<Array<GeoDataset>> {
    return this.GetAll(this.API_ENDPOINT, filter);
  }

  public GetAllShared(filter?: Map<string, string>): Observable<Array<GeoDataset>> {
    return this.GetAll(this.API_ENDPOINT_SHARED, filter);
  }

  public GetAll(apiUrl:string, filter?: Map<string, string>): Observable<Array<GeoDataset>> {
    return this.getSettingsObservable().pipe(
      switchMap(settings => {
        return this.getHttpHeaders().pipe(
          switchMap(httpHeaders => {
            const url = this.getGeoDataURL(settings, apiUrl);
            const httpPrms = this.getFilterQueryStringPrms(filter);            
            return this.http.get<Array<GeoDataset>>(url, { headers: httpHeaders, params: httpPrms });
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

  private getGeoDataSingleURL(urlSettings: ConfigurationSettings, endPoint: string, id: string): string {
    return urlSettings.GeoDataApiUrl + '/' + endPoint + '/' + id;
  }

  private getGeoDataURL(urlSettings: ConfigurationSettings, endPoint: string): string {
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
    if(filter!=null){
      filter.forEach((val,key) => {
        httpPrms = httpPrms.append(key, val);        
      });
    }
    return httpPrms;
  }

}