import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { CoordinateDataMessageBusService } from 'src/app/services/coordinate-data-message-bus.service';
import { debounceTime, delay, takeUntil } from 'rxjs/operators';
import { CoordinateData, Coordinate, PointCollection } from 'src/app/models/coordinate-data';
import { chartFieldSelection } from '../gps-telemetry-chart/gps-telemetry-chart.component';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faLink } from '@fortawesome/free-solid-svg-icons';
import { Subject, Observable } from 'rxjs';

export class chartInstance {
  id: string;
  chartFields: Array<chartFieldSelection>
}

@Component({
  selector: 'app-geo-data-viewer',
  templateUrl: './geo-data-viewer.component.html',
  styleUrls: ['./geo-data-viewer.component.scss']
})
export class GeoDataViewerComponent implements OnInit, OnDestroy {

  public data: CoordinateData = new CoordinateData();
  public mapHeight: string = '200px';

  public selectedChartFields: Array<chartFieldSelection> = [];
  public chartMetadataSelectOptions: chartFieldSelection = new chartFieldSelection();
  public chartTelemetrySelectOptions: chartFieldSelection = new chartFieldSelection();

  public metadataFieldListSelect: Array<string> = [];
  public telemetryFieldListSelect: Array<string> = [];

  public selectedCharts: Array<chartInstance> = [];
  private readonly ChartPreferenceStorageKey = 'chartPref';

  public selectedPointCollection: PointCollection;
  public destroyed = new Subject<boolean>();

  private _selectedSegmentIndex: number = 0;
  get selectedSegmentIndex(): number {
    return this._selectedSegmentIndex;
  }
  @Input()
  set selectedSegmentIndex(idx: number) {
    this._selectedSegmentIndex = idx;
    this.selectedPointCollection = this.data.Data[idx];
    this.selectedPointIndex = 0;
  }

  private _selectedPointIndex: number;
  get selectedPointIndex(): number {
    return this._selectedPointIndex;
  }
  @Input()
  set selectedPointIndex(idx: number) {
    this._selectedPointIndex = idx;
    if (idx != null && this.selectedPointCollection != null) {
      this.selectPoint = this.selectedPointCollection.Coordinates[idx];
    } else {
      console.log('set selectedPointIndex null', idx, this.selectedPointCollection)
    }
  }

  public selectPoint: Coordinate;

  public linkUrl: string;

  constructor(
    private msgService: CoordinateDataMessageBusService, private falibrary: FaIconLibrary) {
    let midHeight = window.innerHeight - 300;
    this.mapHeight = `${midHeight}px`;
    falibrary.addIcons(faLink);
  }

  ngOnInit() {
    this.msgService.subscribeCoordinateDatasetSelected().pipe(debounceTime(500), takeUntil(this.destroyed)).subscribe({
      next: x => {
        this.data = x;
        if (x.Data) {
          this.setChartFields(x);
          this.selectedPointCollection = x.Data[this.selectedSegmentIndex];
          this.selectedSegmentIndex = 0;
          this.linkUrl = `${location.origin}${location.pathname}?id=${x.ID}`;
        }
      }
    });

    this.msgService.subscribePointCollectionIDSelected().pipe(takeUntil(this.destroyed)).subscribe({
      next: id => {
        if (id && this.data && this.data.Data) {
          const idx = this.data.Data.findIndex(x => x.ID === id);
          this.selectedSegmentIndex = idx;
        }
      }
    });

    const storedChartPref = this.getStoredChartPreference();
    if (storedChartPref != null) {
      this.selectedCharts = storedChartPref;
    }
  }

  public copyLinkUrl(element: any): void {
    element.select();
    document.execCommand('copy');
    element.setSelectRange(0, 0);
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
    }
  }

  public addChart() {
    if (this.selectedChartFields != null && this.selectedChartFields.length > 0) {
      this.selectedCharts.push({ id: (this.selectedCharts.length + 1).toString(), chartFields: this.selectedChartFields });
      this.selectedChartFields = [];

      this.storeChartPreference(this.selectedCharts);
    }
  }

  public removeChart(id: string) {
    if (this.selectedCharts != null && this.selectedCharts.length > 0) {
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

  }

  ngOnDestroy(): void {
    this.destroyed.next(true);
  }

}
