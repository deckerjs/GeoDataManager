import { Component, OnInit, ViewChild } from '@angular/core';
import { SettingsService } from './settings.service';
import { NgForm, FormGroup } from '@angular/forms';
// import { ModalController } from '@ionic/angular';
import { URLSettings } from './models/urlsettings';

@Component({
  selector: 'app-portal-settings',
  templateUrl: './portal-settings.page.html',
  styleUrls: ['./portal-settings.page.scss'],
})
export class PortalSettingsPage implements OnInit {

  public GeoManagerAPISetting: string;
  public AuthAPISetting: string;
  constructor(
    private settingsService: SettingsService,
    // private modalCtrl: ModalController
    ){
  }

  ngOnInit() {
    this.settingsService.getURLSettings().subscribe({
      next: settings => {
        console.log("got settings:", settings)
        this.GeoManagerAPISetting = settings.GeoManagerAPI;
        this.AuthAPISetting = settings.AuthAPI;
      }
    });
  }

  public onSaveSettings() {
    const settings = new URLSettings();
    settings.AuthAPI = this.AuthAPISetting;
    settings.GeoManagerAPI = this.GeoManagerAPISetting;
    this.settingsService.saveURLSettings(settings);
  }

}
