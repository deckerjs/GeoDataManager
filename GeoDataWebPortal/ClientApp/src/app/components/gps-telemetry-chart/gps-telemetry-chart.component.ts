import { Component, OnInit, Input } from '@angular/core';
import { GpsTelemetry, Coordinate } from 'src/app/models/coordinate-data';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';


export interface fieldDataDictionary { [key: string]: [] }

@Component({
  selector: 'app-gps-telemetry-chart',
  templateUrl: './gps-telemetry-chart.component.html',
  styleUrls: ['./gps-telemetry-chart.component.scss']
})
export class GpsTelemetryChartComponent implements OnInit {

  public lineChartData: ChartDataSets[];
  public lineChartLabels: Label[];
  public lineChartOptions: (ChartOptions & { annotation: any });
  public lineChartColors: Color[];
  public lineChartLegend: boolean;
  public lineChartType: string;
  public lineChartPlugins: [] = [];

  private _dataObject: string;
  get dataObject(): string {
    return this._dataObject;
  }
  @Input()
  set dataObject(dob: string) {
    this._dataObject = dob;
    if (dob != null && this.isFieldsReady() && this.isDataReady()) {
      this.chartSetup(this.data, dob, this.dataFields);
    }
  }

  private _dataFields: Array<string>;
  get dataFields(): Array<string> {
    return this._dataFields;
  }
  @Input()
  set dataFields(df: Array<string>) {
    this._dataFields = df;
    if (df != null && df.length > 0 && this.isDataReady() && this.isDataObjectReady()) {
      this.chartSetup(this.data, this.dataObject, df);
    }
  }

  private _data: Array<Coordinate>;
  get data(): Array<Coordinate> {
    return this._data;
  }
  @Input()
  set data(tdata: Array<Coordinate>) {
    this._data = tdata;
    if (tdata != null && tdata.length > 0 && this.isFieldsReady() && this.isDataObjectReady()) {
      this.chartSetup(tdata, this.dataObject, this.dataFields);
    }
  }

  constructor() { }
  ngOnInit(): void { }

  private isDataReady(): boolean {
    return this.data != null && this.data.length > 0;
  }

  private isFieldsReady(): boolean {
    return this.dataFields != null && this.dataFields.length > 0;
  }

  private isDataObjectReady(): boolean {
    return this.dataObject != null;
  }

  private chartSetup(tdata: Array<Coordinate>, dataObject: string, dataFields: Array<string>) {
    this.lineChartData = this.getDataSets(tdata, dataObject, dataFields);
    this.lineChartLabels = this.getDataTimeLabels(tdata);

    this.lineChartOptions = {
      responsive: true,
      annotation: '?'
    };

    this.lineChartColors = [
      {
        borderColor: 'black',
        backgroundColor: 'rgba(0,255,0,0.5)',
      },
    ];

    this.lineChartLegend = true;
    this.lineChartType = 'line';
    this.lineChartPlugins = [];
  }

  private getDataSets(tdata: Array<Coordinate>, dataObject: string, dataFields: Array<string>): ChartDataSets[] {
    console.log('getDataSets:', tdata, dataObject, dataFields)
    let datas: ChartDataSets[] = dataFields.map(x => {
      const fd = tdata.map(td => {
        const dob = td[dataObject];
        if (dob != null) {
          return dob[x];
        } else {
          return null;
        }
      });

      return {
        label: x,
        data: fd,
        backgroundColor: 'rgba(0,0,255,1)',
        borderColor: 'black',
        fill: true
      }
    })

    return datas;
  }

  private getDataTimeLabels(tdata: Array<Coordinate>): Array<string> {
    let allDates = tdata.map(x => x.Time);
    let coordTimes: Array<string> = [];
    allDates.forEach((x) => {
      let dt = new Date(x);
      // coordTimes.push(dt.toLocaleTimeString('en', { year: 'numeric', month: 'numeric', day: 'numeric' }))
      // let ts = `${dt.getHours()}:${dt.getMinutes()}:${dt.getSeconds()}:${dt.getMilliseconds()}` 
      coordTimes.push(dt.toLocaleTimeString())
    });

    return coordTimes;
  }
}
