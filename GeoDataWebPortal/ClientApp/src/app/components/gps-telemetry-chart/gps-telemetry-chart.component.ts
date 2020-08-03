import { Component, OnInit, Input } from '@angular/core';
import { GpsTelemetry, Coordinate } from 'src/app/models/coordinate-data';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';


export interface fieldDataDictionary { [key: string]: [] }

export class chartFieldSelection {
  dataObject: string;
  dataFields: Array<string>;
}

@Component({
  selector: 'app-gps-telemetry-chart',
  templateUrl: './gps-telemetry-chart.component.html',
  styleUrls: ['./gps-telemetry-chart.component.scss']
})
export class GpsTelemetryChartComponent implements OnInit {

  public lineChartData: ChartDataSets[] = [];
  public lineChartLabels: Label[] = [];
  public lineChartOptions: (ChartOptions & { annotation: any });
  public lineChartColors: Color[] = [];
  public lineChartLegend: boolean;
  public lineChartType: string;
  public lineChartPlugins: [] = [];


  private _dataFields: Array<chartFieldSelection>;
  get dataFields(): Array<chartFieldSelection> {
    return this._dataFields;
  }
  @Input()
  set dataFields(df: Array<chartFieldSelection>) {
    // console.log('setting datafields:', df)
    this._dataFields = df;
    if (df != null && df.length > 0 && this.isDataReady()) {
      this.chartSetup(this.data, df);
    }
  }

  private _data: Array<Coordinate>;
  get data(): Array<Coordinate> {
    return this._data;
  }
  @Input()
  set data(tdata: Array<Coordinate>) {
    this._data = tdata;
    if (tdata != null && tdata.length > 0 && this.isFieldsReady()) {
      this.chartSetup(tdata, this.dataFields);
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

  private chartSetup(tdata: Array<Coordinate>, dataFields: Array<chartFieldSelection>) {
    // console.log('chartSetup:', tdata, dataFields)
    
    dataFields.forEach(cf => {
      this.lineChartData.push(...this.getDataSets(tdata, cf));      
    });

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

  private getDataSets(tdata: Array<Coordinate>, dataFields: chartFieldSelection): ChartDataSets[] {
    // console.log('getDataSets:', tdata, dataFields)
    
    let datas: ChartDataSets[] = dataFields.dataFields.map(chartFld => {
      const fd = tdata.map(td => {
        
          const dob = td[dataFields.dataObject];
          if (dob != null) {
            return dob[chartFld];
          } else{
            return null;
          }        
      });

      return {
        label: chartFld,
        data: fd,
        backgroundColor: 'rgba(0,0,255,1)',
        borderColor: 'black',
        fill: true
      }

    })
// console.log('returning datas:', datas)
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
