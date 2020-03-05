import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-empty-module-shell',
  templateUrl: './empty-module-shell.component.html',
  styleUrls: ['./empty-module-shell.component.scss']
})
export class EmptyModuleShellComponent implements OnInit {
  @Input() topRowHeight: number = 50;
  @Input() sideNavWidth: number = 200;
  @Input() mainContentScroll: string = "hidden"

  constructor() {
    console.log(window.screen.availHeight);
    console.log(window.innerHeight);
  }

  ngOnInit() {}
  ngAfterViewInit() {
    this.resetSizeValues();
  }
  
  onResize(event) {
    this.resetSizeValues();
  }

  private resetSizeValues() {
  }

  public closeSideNav(){
    if (this.sideNavWidth===0){
    }
    else
    {
    }

  }
}
