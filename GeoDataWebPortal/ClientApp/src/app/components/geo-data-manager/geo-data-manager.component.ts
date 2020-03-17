import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-geo-data-manager",
  templateUrl: "./geo-data-manager.component.html",
  styleUrls: ["./geo-data-manager.component.scss"]
})
export class GeoDataManagerComponent implements OnInit {
  
  public showEditor: boolean = true;
  public showViewer: boolean = true;
  public showImport: boolean = true;

  ngOnInit() { }
}

