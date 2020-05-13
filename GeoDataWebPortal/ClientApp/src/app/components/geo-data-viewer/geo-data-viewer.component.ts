import { Component, OnInit } from '@angular/core';
import { CoordinateDataMessageBusService } from 'src/app/services/coordinate-data-message-bus.service';
import { debounceTime, delay } from 'rxjs/operators';
import { CoordinateData } from 'src/app/models/coordinate-data';

export class chartSelection {
  dataObject: string;
  dataFields: Array<string>;
}

@Component({
  selector: 'app-geo-data-viewer',
  templateUrl: './geo-data-viewer.component.html',
  styleUrls: ['./geo-data-viewer.component.scss']
})
export class GeoDataViewerComponent implements OnInit {

  public data: CoordinateData = new CoordinateData();
  public mapHeight: string = '200px';

  public selectedChartFields: Array<chartSelection> = [];
  public chartMetadataSelectOptions: chartSelection = new chartSelection();
  public chartTelemetrySelectOptions: chartSelection = new chartSelection();

  public metadataFieldListSelect: Array<string> = [];
  public telemetryFieldListSelect: Array<string> = [];

  constructor(
    private msgService: CoordinateDataMessageBusService
  ) {
    let midHeight = window.innerHeight - 300;
    this.mapHeight = `${midHeight}px`;
  }

  ngOnInit() {
    this.msgService.subscribeCoordinateDatasetSelected().pipe(debounceTime(500)).subscribe(x => {
      this.data = x;

      if (x.Data) {
        this.setChartFields(x);
      }

    });
  }

  public selectMetaDataFld(fld: string) {
    this.metadataFieldListSelect.push(fld);
  }

  public selectTelemetryDataFld(fld: string) {
    this.telemetryFieldListSelect.push(fld);
  }

  public addSelectedField(dob: string, flds: string[]) {
    this.selectedChartFields.push({ dataObject: dob, dataFields: flds });
    console.log('added flds:', this.selectedChartFields)
  }

  private setChartFields(data: CoordinateData) {
    const dataIdx = data.Data.length - 1;
    const coordIdx = data.Data[dataIdx].Coordinates.length - 1;

    let dataItem = data.Data[dataIdx].Coordinates[coordIdx];

    let metadataProps: string[] = [];
    let telemetryProps: string[] = [];

    console.log('dataItem : ', dataItem)

    this.chartMetadataSelectOptions.dataObject = 'Metadata';
    this.chartMetadataSelectOptions.dataFields = [];
    for (let prop1 in dataItem['Metadata']) {
      this.chartMetadataSelectOptions.dataFields.push(prop1);
    }

    this.chartTelemetrySelectOptions.dataObject = 'Telemetry';
    this.chartTelemetrySelectOptions.dataFields = [];
    for (let prop1 in dataItem['Telemetry']) {
      this.chartTelemetrySelectOptions.dataFields.push(prop1);
    }

    console.log('this.chartDataSelectOptions : ', this.chartMetadataSelectOptions)
    console.log('this.chartDataSelectOptions : ', this.chartTelemetrySelectOptions)
  }

}
