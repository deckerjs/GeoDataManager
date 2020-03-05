import { Component, OnInit, Input } from "@angular/core";
import { GeoDataAPIService } from 'src/app/services/geo-data-api.service';

@Component({
  selector: "app-main-view",
  templateUrl: "./main-view.component.html",
  styleUrls: ["./main-view.component.scss"]
})
export class MainViewComponent implements OnInit {
  @Input()
  topRowHeight: number = 65;

  public apiHealthy:boolean=false;

  constructor(private dataService: GeoDataAPIService) {

  }

  ngOnInit() {

    this.dataService.apiHealthCheckPolling().subscribe(
      {
        next:
          x => {            
            this.apiHealthy = (x=='Healthy');
          },
          error:e=>{
            console.log("polling health error:", e);
            this.apiHealthy = false;
          },
          complete:()=>{
            console.log("polling health completed");
          }
      }
    );

  }
}