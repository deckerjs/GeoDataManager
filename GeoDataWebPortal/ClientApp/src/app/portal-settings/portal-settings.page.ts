import { Component, OnInit, ViewChild } from '@angular/core';
import { SettingsService } from './settings.service';
import { ConfigurationSettings } from './models/urlsettings';

@Component({
  selector: 'app-portal-settings',
  templateUrl: './portal-settings.page.html',
  styleUrls: ['./portal-settings.page.scss'],
})
export class PortalSettingsPage implements OnInit {

  public GeoManagerAPISetting: string;
  public AuthAPISetting: string;
  constructor(private settingsService: SettingsService){}

  ngOnInit() {
    this.settingsService.getSettings().subscribe({
      next: settings => {
        this.GeoManagerAPISetting = settings.GeoDataApiUrl;
        this.AuthAPISetting = settings.AuthUrl;
      }
    });
  }

  public onSaveSettings() {
    const settings = new ConfigurationSettings();
    settings.AuthUrl = this.AuthAPISetting;
    settings.GeoDataApiUrl = this.GeoManagerAPISetting;
    this.settingsService.saveURLSettings(settings);
  }

}
