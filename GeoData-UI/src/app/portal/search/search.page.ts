import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.page.html',
  styleUrls: ['./search.page.scss'],
})
export class SearchPage implements OnInit {

public searchResults:any = [
  {col1:'col1 something1', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something2', col2:'col2 something2', col3:'col3 something2'},
  {col1:'col1 something3', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something4', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something5', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something6', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something1', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something2', col2:'col2 something2', col3:'col3 something2'},
  {col1:'col1 something3', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something4', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something5', col2:'col2 something', col3:'col3 something'},
  {col1:'col1 something6', col2:'col2 something', col3:'col3 something'}
];

  constructor() { }

  ngOnInit() {
  }

}
