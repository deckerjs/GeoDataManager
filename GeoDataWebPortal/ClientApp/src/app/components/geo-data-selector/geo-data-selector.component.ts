import { Component, OnInit } from '@angular/core';
import {
  GeoDataMessageBusService,
  MessageType
} from '../../services/geo-data-message-bus.service';
import { GeoDataset } from '../../models/geo-dataset';
import { GeoDataAPIService } from 'src/app/services/geo-data-api.service';
import { AuthService } from 'src/app/auth/auth.service';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faSync } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-geo-data-selector',
  templateUrl: './geo-data-selector.component.html',
  styleUrls: ['./geo-data-selector.component.scss']
})
export class GeoDataSelectorComponent implements OnInit {
  public datasetCollection: Array<GeoDataset>;
  public selectedDataset: GeoDataset;

  constructor(    
    private apiDataService: GeoDataAPIService,
    private msgService: GeoDataMessageBusService,
    private authService: AuthService,
    private falibrary: FaIconLibrary
  ) {
    this.datasetCollection = [];
    falibrary.addIcons(faSync);
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
      })
  }

  private clearDataset() {
    this.datasetCollection = [];
  }

  private loadDatasetsFromRepo() {
    this.datasetCollection = [];

    this.apiDataService.GetAll().subscribe({
      next: x => {
        console.log("pushing api data collection", x)
        this.datasetCollection.push(...x);
      }
    });
  }

  public selectDataset(item: GeoDataset) {
    this.selectedDataset = item;
    this.msgService.publishGeoDatasetSelected(item);
  }

  public reload(): void {
    this.loadDatasetsFromRepo();
  }

}
