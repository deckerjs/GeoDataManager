import { Component, OnInit } from '@angular/core';
import { SettingsService } from 'src/app/portal-settings/settings.service';
import { GeoDataMessageBusService } from 'src/app/services/geo-data-message-bus.service';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faTrash, faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { ConfigurationSettings } from 'src/app/services/configuration-settings.service';
import { UserSettingsAPIService } from 'src/app/services/user-settings-api.service';
import { UserDataPermission } from 'src/app/models/user-data-permission';
import { AppUser } from 'src/app/models/app-user';

@Component({
  selector: 'app-data-settings',
  templateUrl: './data-settings.component.html',
  styleUrls: ['./data-settings.component.scss']
})
export class DataSettingsComponent implements OnInit {

  configurationSettings: ConfigurationSettings;
  userDataPermissions: UserDataPermission[] = [];
  userDataGrantedPermissions: UserDataPermission[] = [];
  userList: AppUser[] = [];

  constructor(
    private settingsService: SettingsService,
    private dataService: UserSettingsAPIService,
    private msgService: GeoDataMessageBusService,
    private falibrary: FaIconLibrary) {
    falibrary.addIcons(faTrash, faPlusSquare);
  }

  ngOnInit(): void {
    this.refreshConfigurationSettings();
    this.refreshUserData();
    this.refreshPermissionData();
  }

  public addDataPermission(id: string): void {
    let newData: UserDataPermission = {
      ID: null,
      AllowedUserID: id,
      OwnerUserID: null,
      Read: true,
      ResourceName: null,
      Write: false
    }
    this.dataService.createUserDataPermission(newData).subscribe({
      next: x => {
        this.refreshPermissionData();
      }
    });
  }

  public removeDataPermission(id: string): void {
    this.dataService.deleteUserDataPermission(id).subscribe({
      next: x => {
        this.refreshPermissionData();
      }
    });
  }

  public getUserNameFromId(id: string): string {
    return this.userList.find(x => x.ID == id).UserName;
  }

  public refreshPermissionData(): void {
    this.dataService.getAllUserDataPermissions().subscribe({
      next: x => {
        this.userDataPermissions = x;
      }
    });

    this.dataService.getAllGrantedDataPermissions().subscribe({
      next: x => {
        this.userDataGrantedPermissions = x;
      }
    });
  }

  public refreshUserData(): void {
    this.dataService.getAllUsers().subscribe({
      next: x => {
        this.userList = x;
      }
    });
  }

  public refreshConfigurationSettings(): void {
    this.settingsService.getSettings().subscribe({
      next: x => {
        this.configurationSettings = x;
      }
    });
  }

}
