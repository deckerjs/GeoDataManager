import { Component, OnInit } from '@angular/core';
import { GeoDataset } from 'src/app/models/geo-dataset';
import { GeoDataAPIService } from 'src/app/services/geo-data-api.service';
import { GeoDataMessageBusService } from 'src/app/services/geo-data-message-bus.service';
import { debounceTime, delay } from 'rxjs/operators';

@Component({
  selector: 'app-geo-data-viewer',
  templateUrl: './geo-data-viewer.component.html',
  styleUrls: ['./geo-data-viewer.component.scss']
})
export class GeoDataViewerComponent implements OnInit {

  public data: GeoDataset = new GeoDataset();
  public mapHeight: string = '200px';

  constructor(
    private dataService: GeoDataAPIService,
    private msgService: GeoDataMessageBusService
  ) { 
    let midHeight = window.innerHeight - 300;
    this.mapHeight = `${midHeight}px`;
  }

  ngOnInit() {
    this.msgService.subscribeGeoDatasetSelected().pipe(debounceTime(500)).subscribe(x => {
      this.data = x;
    });
  }



}
