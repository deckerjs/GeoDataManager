import { Injectable } from '@angular/core';
import { ConfigurationSettings } from './models/urlsettings';
import { environment } from 'src/environments/environment';
import { from, Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private readonly API_ENDPOINT: string = '/api/ConfigurationSettings';
  private _settings: ConfigurationSettings;

  constructor(private http: HttpClient) {}

  public getSettings(): Observable<ConfigurationSettings> {
    console.log('get settings')
    return this.http.get<ConfigurationSettings>(this.API_ENDPOINT);
  }

  public saveURLSettings(settings: ConfigurationSettings): void {
    this.localStoreURLSettings(settings);
  }

  private localStoreURLSettings(settings: ConfigurationSettings): void {
    const data = JSON.stringify({
      AuthAPI: settings.AuthUrl,
      GeoManagerAPI: settings.GeoDataApiUrl
    });    
  }

  private localLoadURLSettings(): Observable<ConfigurationSettings> {
    //todo: hookup to local storage
    return null;
    // console.log("attempting load of local url settings")
    // return from(Plugins.Storage.get({ key: 'urlSettings' })).pipe(map(data => {
    //   console.log("local settings", data)
    //   if (!data || !data.value) {
    //     console.log("local settings empty, returning null", data)
    //     return null;
    //   } else {
    //     console.log("local settings not empty returning value from: ", data)
    //     return <URLSettings>JSON.parse(data.value) as {
    //       AuthAPI: string;
    //       GeoManagerAPI: string;
    //     };
    //   }
    // }));
  }
}
