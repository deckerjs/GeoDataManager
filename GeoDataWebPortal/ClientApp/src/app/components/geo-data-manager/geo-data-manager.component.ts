import { Component, OnInit, Input } from "@angular/core";
import { CoordinateDataAPIService } from 'src/app/services/coordinate-data-api.service';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';
import { CoordinateDataMessageBusService } from 'src/app/services/coordinate-data-message-bus.service';

@Component({
  selector: "app-geo-data-manager",
  templateUrl: "./geo-data-manager.component.html",
  styleUrls: ["./geo-data-manager.component.scss"]
})
export class GeoDataManagerComponent implements OnInit {

  public showEditor: boolean = true;
  public showViewer: boolean = true;
  public showImport: boolean = true;
  public showSettings: boolean = true;

  constructor(
    private apiDataService: CoordinateDataAPIService, 
    private msgService: CoordinateDataMessageBusService,
    private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      var id = params['id'];
      if (id) {
        this.apiDataService.Get(id).pipe(take(1)).subscribe({next: dataset=>{
          this.msgService.publishCoordinateDatasetSelected(dataset);
        }})
      }
    });
  }
}

