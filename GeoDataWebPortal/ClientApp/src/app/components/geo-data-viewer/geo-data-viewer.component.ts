import { Component, OnInit } from '@angular/core';
import { CoordinateDataMessageBusService } from 'src/app/services/coordinate-data-message-bus.service';
import { debounceTime, delay } from 'rxjs/operators';
import { CoordinateData } from 'src/app/models/coordinate-data';
import { chartFieldSelection } from '../gps-telemetry-chart/gps-telemetry-chart.component';

export class chartInstance {
  id: string;
  chartFields: Array<chartFieldSelection>
}

@Component({
  selector: 'app-geo-data-viewer',
  templateUrl: './geo-data-viewer.component.html',
  styleUrls: ['./geo-data-viewer.component.scss']
})
export class GeoDataViewerComponent implements OnInit {

  public data: CoordinateData = new CoordinateData();
  public mapHeight: string = '200px';

  public selectedChartFields: Array<chartFieldSelection> = [];
  public chartMetadataSelectOptions: chartFieldSelection = new chartFieldSelection();
  public chartTelemetrySelectOptions: chartFieldSelection = new chartFieldSelection();

  public metadataFieldListSelect: Array<string> = [];
  public telemetryFieldListSelect: Array<string> = [];

  public selectedCharts: Array<chartInstance> = [];

  private readonly ChartPreferenceStorageKey = 'chartPref';

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

    const storedChartPref = this.getStoredChartPreference();
    if (storedChartPref != null) {
      this.selectedCharts = storedChartPref;
    }
  }

  private storeChartPreference(charts: Array<chartInstance>) {
    if (charts != null && charts.length > 0) {
      const localChartPrefString = JSON.stringify(charts);
      localStorage.setItem(this.ChartPreferenceStorageKey, localChartPrefString);
    }
  }

  private getStoredChartPreference(): Array<chartInstance> {
    let charts: Array<chartInstance>;
    const localChartPrefString = localStorage.getItem(this.ChartPreferenceStorageKey);

    if (localChartPrefString != null) {
      charts = JSON.parse(localChartPrefString) as Array<chartInstance>;
    }

    return charts;
  }

  public selectMetaDataFld(fld: string) {
    this.metadataFieldListSelect.push(fld);
  }

  public selectTelemetryDataFld(fld: string) {
    this.telemetryFieldListSelect.push(fld);
  }

  public addSelectedField(dob: string, flds: string[]) {
    if (dob != null && flds != null && flds.length > 0) {
      this.selectedChartFields.push({ dataObject: dob, dataFields: flds });
      this.metadataFieldListSelect = [];
      this.telemetryFieldListSelect = [];
      console.log('added flds:', this.selectedChartFields)
    }
  }

  public addChart() {
    if (this.selectedChartFields != null && this.selectedChartFields.length > 0) {
      this.selectedCharts.push({ id: (this.selectedCharts.length + 1).toString(), chartFields: this.selectedChartFields });
      this.selectedChartFields = [];
      console.log('added chart:', this.selectedCharts)

      this.storeChartPreference(this.selectedCharts);
    }
  }

  public removeChart(id: string) {
    if(this.selectedCharts!=null && this.selectedCharts.length>0){
      this.selectedCharts = this.selectedCharts.filter(item => item.id !== id);
      this.storeChartPreference(this.selectedCharts);
    }
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
