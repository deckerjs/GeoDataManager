import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CoordinateDataSummary } from 'src/app/models/coordinate-data';


@Component({
  selector: 'app-coordinate-data-summary-item',
  templateUrl: './coordinate-data-summary-item.component.html',
  styleUrls: ['./coordinate-data-summary-item.component.scss']
})
export class CoordinateDataSummaryItemComponent implements OnInit {

  @Input() summaryData: CoordinateDataSummary;
  @Output() selected = new EventEmitter<CoordinateDataSummary>();

  constructor() { }

  ngOnInit(): void {
  }

  public onSelect():void{
    this.selected.emit(this.summaryData)
  }


}
