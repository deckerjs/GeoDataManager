import { Injectable } from '@angular/core';
import { URLSettings } from './models/urlsettings';
import { environment } from 'src/environments/environment';
import { Plugins } from '@capacitor/core';
import { from, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

const { Storage } = Plugins;

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor() {
    this._urlSettings = this.getDefaultSettings();
  }

  private _urlSettings: URLSettings;

  private getDefaultSettings(): URLSettings {    
    const urlSettings = new URLSettings();
    urlSettings.GeoManagerAPI = environment.geoManagerAPI;
    urlSettings.AuthAPI = environment.authapiurl;
    console.log("set default url settings", urlSettings)
    return urlSettings;
  }

  public getURLSettings(): Observable<URLSettings> {
    return this.localLoadURLSettings().pipe(map(data => {
      if (!data) {
        console.log("returning defaults:", this._urlSettings)
        return this._urlSettings;
      } else {
        console.log("returning from local store:", data)
        return data;
      }
    }
    ));
  }

  public saveURLSettings(settings: URLSettings):void {
    this.localStoreURLSettings(settings);
  }

  private localStoreURLSettings(settings: URLSettings):void {
    const data = JSON.stringify({
      AuthAPI: settings.AuthAPI,
      GeoManagerAPI: settings.GeoManagerAPI
    });
    Plugins.Storage.set({ key: 'urlSettings', value: data });
  }

  private localLoadURLSettings(): Observable<URLSettings> {
    console.log("attempting load of local url settings")
    return from(Plugins.Storage.get({ key: 'urlSettings' })).pipe(map(data => {
      console.log("local settings", data)
      if (!data || !data.value) {
        console.log("local settings empty, returning null", data)
        return null;
      } else {
        console.log("local settings not empty returning value from: ", data)
        return <URLSettings>JSON.parse(data.value) as {
          AuthAPI: string;
          GeoManagerAPI: string;
        };
      }
    }));
  }
}
