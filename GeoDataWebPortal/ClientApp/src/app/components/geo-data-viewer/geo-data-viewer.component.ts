import { Component, OnInit } from '@angular/core';
import { CoordinateDataMessageBusService } from 'src/app/services/coordinate-data-message-bus.service';
import { debounceTime, delay } from 'rxjs/operators';
import { CoordinateData } from 'src/app/models/coordinate-data';

@Component({
  selector: 'app-geo-data-viewer',
  templateUrl: './geo-data-viewer.component.html',
  styleUrls: ['./geo-data-viewer.component.scss']
})
export class GeoDataViewerComponent implements OnInit {

  public data: CoordinateData = new CoordinateData();
  public mapHeight: string = '200px';

  constructor(    
    private msgService: CoordinateDataMessageBusService
  ) { 
    let midHeight = window.innerHeight - 300;
    this.mapHeight = `${midHeight}px`;
  }

  ngOnInit() {
    this.msgService.subscribeCoordinateDatasetSelected().pipe(debounceTime(500)).subscribe(x => {
      this.data = x;
    });
  }

}
