import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { GpsTelemetry, Coordinate } from 'src/app/models/coordinate-data';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-gps-telemetry-chart',
  templateUrl: './gps-telemetry-chart.component.html',
  styleUrls: ['./gps-telemetry-chart.component.scss']
})
export class GpsTelemetryChartComponent implements OnInit {

  constructor(private elementRef: ElementRef) { }

  public chart = [];

  private _data: Array<Coordinate>;
  get data(): Array<Coordinate> {
    return this._data;
  }
  @Input()
  set data(tdata: Array<Coordinate>) {
    this._data = tdata;
    if (tdata != null) {
      this.updateChart(tdata);
    }
  }

  ngOnInit(): void {

  }

  private updateChart(tdata: Array<Coordinate>) {
    let htmlRef = this.elementRef.nativeElement.querySelector('#chartCanvas');

    let allDates = tdata.map(x => x.Time);
    // let quality = tdata.map(x => x.Telemetry.Quality);
    let signalToNoiseRatio = tdata.map(x => x.Telemetry.SignalToNoiseRatio);
    let fps = tdata.map(x => x.Telemetry.FeetPerSecond);
    let hdop = tdata.map(x => x.Telemetry.Hdop);

    let coordTimes = [];

    allDates.forEach((x) => {
      let dt = new Date(x);
      coordTimes.push(dt.toLocaleTimeString('en', { year: 'numeric', month: 'numeric', day: 'numeric' }))
    })

    this.chart = new Chart(htmlRef, {
      type: 'line',
      data: {
        labels: coordTimes,
        datasets: [
          {
            label: 'STN Ratio',
            data: signalToNoiseRatio,
            borderColor: '#3cba9f',
            fill: false
          },
          {
            label: 'FPS',
            data: fps,
            borderColor: '#5fbb00',
            fill: false
          },
          {
            label: 'HDOP',
            data: hdop,
            borderColor: '#7ecc00',
            fill: false
          }
        ]
      },
      options: {
        legend: {
          display: true
        },
        scales: {
          xAxes: [{
            display: true
          }],
          yAxes: [{
            display: true
          }]
        }
      }
    });
    
  }

}
