import { Component, OnInit } from '@angular/core';
import {
  GeoDataMessageBusService,
  MessageType
} from '../../services/geo-data-message-bus.service';
import { GeoDataset } from '../../models/geo-dataset';
import { GeoDataAPIService } from 'src/app/services/geo-data-api.service';
import { AuthService } from 'src/app/auth/auth.service';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faSync, faChevronDown, faChevronRight } from '@fortawesome/free-solid-svg-icons';
import { AppUser } from 'src/app/models/app-user';
import { UserSettingsAPIService } from 'src/app/services/user-settings-api.service';
import { stringify } from 'querystring';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-geo-data-selector',
  templateUrl: './geo-data-selector.component.html',
  styleUrls: ['./geo-data-selector.component.scss']
})
export class GeoDataSelectorComponent implements OnInit {
  public datasetCollection: Array<GeoDataset>;
  public selectedDataset: GeoDataset;
  public userList: AppUser[] = [];
  public showFilter: boolean = false;
  public filterShowShared: boolean = false;
  public filterForSelectedUser: AppUser;

  constructor(
    private apiDataService: GeoDataAPIService,
    private msgService: GeoDataMessageBusService,
    private authService: AuthService,
    private userDataService: UserSettingsAPIService,
    private falibrary: FaIconLibrary
  ) {
    this.datasetCollection = [];
    falibrary.addIcons(faSync, faChevronDown, faChevronRight);
  }

  ngOnInit() {
    this.loadDatasetsFromRepo();

    this.msgService
      .subscribeGeneral(MessageType.NewGeoDataAvailable)
      .subscribe(x => {
        this.loadDatasetsFromRepo();
      });

    this.authService.userIsAuthenticated
      .subscribe({
        next: x => {
          if (x) {
            this.loadDatasetsFromRepo();
          } else {
            this.clearDataset();
          }
        }
      });
  }

  private clearDataset() {
    this.datasetCollection = [];
  }

  private loadDatasetsFromRepo() {
    this.datasetCollection = [];

    this.refreshUserData().subscribe({
      next: u => {
        this.apiDataService.GetAll().subscribe({
          next: x => {
            console.log("pushing api data collection", x)
            this.datasetCollection.push(...x);
          }
        });
      }
    });
  }

  public refreshUserData(): Observable<AppUser[]> {
    return this.userDataService.getAllUsers().pipe(tap(
      x => {
        this.userList = x;
      }
    ));
  }

  public getUserNameFromId(id: string): string {
    return this.userDataService.getUserNameFromId(id, this.userList);
  }

  public selectDataset(item: GeoDataset) {
    this.selectedDataset = item;
    this.msgService.publishGeoDatasetSelected(item);
  }

  public reload(): void {
    this.loadDatasetsFromRepo();
  }

  public filterToggle(): void {
    this.showFilter = !this.showFilter;
  }

}
