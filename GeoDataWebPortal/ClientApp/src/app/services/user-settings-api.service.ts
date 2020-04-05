import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

import { Observable, of, Subject } from 'rxjs';
import { SettingsService } from '../portal-settings/settings.service';
import { AuthService } from '../auth/auth.service';
import { switchMap, delay, repeat, tap, catchError } from 'rxjs/operators';
import { ConfigurationSettings } from '../portal-settings/models/urlsettings';
import { UserDataPermission } from '../models/user-data-permission';

@Injectable({
  providedIn: 'root'
})

export class UserSettingsAPIService {

  private readonly API_MAIN_ENDPOINT: string = 'api/User';
  private readonly DATA_PERMISSIONS_ENDPOINT: string = 'DataPermissions';
  private readonly DATA_PERMISSIONS_GRANTED_ENDPOINT: string = 'DataPermissions/Granted';  

  constructor(
    private http: HttpClient,
    private settingsService: SettingsService,
    private authService: AuthService) { }

    public getUserDataPermission(id: string): Observable<UserDataPermission> {
        return this.getSettingsObservable().pipe(
          switchMap(settings => {
            return this.getHttpHeaders().pipe(
              switchMap(httpHeaders => {
                const url = this.getUserApiURL(settings, this.DATA_PERMISSIONS_ENDPOINT) + '/' + id;
                return this.http.get<UserDataPermission>(url, { headers: httpHeaders });
              }));
          })
        );
      }
    
      public getAllUserDataPermissions(filter?: string): Observable<Array<UserDataPermission>> {
        return this.getSettingsObservable().pipe(
          switchMap(settings => {
            return this.getHttpHeaders().pipe(
              switchMap(httpHeaders => {
                const url = this.getUserApiURL(settings, this.DATA_PERMISSIONS_ENDPOINT);
                const httpPrms = new HttpParams();
                httpPrms.append('filter', filter);
                return this.http.get<Array<UserDataPermission>>(url, { headers: httpHeaders, params: httpPrms });
              }));
          })
        );
      }

      public update(data: UserDataPermission) {
        return this.getSettingsObservable().pipe(
          switchMap(settings => {
            return this.getHttpHeaders().pipe(
              switchMap(httpHeaders => {
                const url = this.getUserApiURL(settings, this.DATA_PERMISSIONS_ENDPOINT);
                console.log("put:", data, " url:", url, "headers:", Headers)
                return this.http.put<UserDataPermission>(url, data, { headers: httpHeaders });
              }));
          })
        );
      }

      public deleteUserDataPermission(id: string): Observable<any> {
        return this.getSettingsObservable().pipe(
          switchMap(settings => {
            return this.getHttpHeaders().pipe(
              switchMap(httpHeaders => {
                const url = this.getUserApiURL(settings, this.DATA_PERMISSIONS_ENDPOINT) + '/' + id;
                return this.http.delete(url, { headers: httpHeaders });
              }));
          })
        );
      }

      public getAllGrantedDataPermissions(filter?: string): Observable<Array<UserDataPermission>> {
        return this.getSettingsObservable().pipe(
          switchMap(settings => {
            return this.getHttpHeaders().pipe(
              switchMap(httpHeaders => {
                const url = this.getUserApiURL(settings, this.DATA_PERMISSIONS_GRANTED_ENDPOINT);
                const httpPrms = new HttpParams();
                httpPrms.append('filter', filter);
                return this.http.get<Array<UserDataPermission>>(url, { headers: httpHeaders, params: httpPrms });
              }));
          })
        );
      }

      private getUserApiURL(urlSettings: ConfigurationSettings, endPoint: string): string {
        return urlSettings.GeoDataApiUrl + '/' +  this.API_MAIN_ENDPOINT + '/' + endPoint;
      }

      private getSettingsObservable(): Observable<ConfigurationSettings> {
        return this.settingsService.getSettings();
      }
    
      private getHttpHeaders(): Observable<HttpHeaders> {
        return this.authService.token.pipe(switchMap(beartoken => {
          return of(this.getHeaderOptions(beartoken));
        }));
      }

      private getHeaderOptions(token: string): HttpHeaders {
        var httpHeaders = new HttpHeaders({
          'Content-Type': 'application/json; charset=utf-8',
          'Authorization': 'Bearer ' + token
        });
        return httpHeaders;
      }

  }