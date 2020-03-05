import { Injectable } from '@angular/core';
import { URLSettings } from './models/urlsettings';
import { environment } from 'src/environments/environment';
import { from, Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor(private ConfigurationSettingsService ) {
    // this._urlSettings = this.getDefaultSettings();
  }

  private _urlSettings: URLSettings;

  // private getDefaultSettings(): URLSettings {
  //   const urlSettings = new URLSettings();
  //   urlSettings.GeoManagerAPI = environment.geoManagerAPI;
  //   urlSettings.AuthAPI = environment.authapiurl;
  //   console.log("set default url settings", urlSettings)
  //   return urlSettings;
  // }

  public getURLSettings(): Observable<URLSettings> {

    return of(this._urlSettings);

  }

  public saveURLSettings(settings: URLSettings): void {
    this.localStoreURLSettings(settings);
  }

  private localStoreURLSettings(settings: URLSettings): void {
    const data = JSON.stringify({
      AuthAPI: settings.AuthAPI,
      GeoManagerAPI: settings.GeoManagerAPI
    });    
  }

  private localLoadURLSettings(): Observable<URLSettings> {
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
