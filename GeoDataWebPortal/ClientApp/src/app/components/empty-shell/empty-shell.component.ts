import { Component, OnInit, Input } from '@angular/core';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faCaretSquareLeft, faCaretSquareRight } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-empty-shell',
  templateUrl: './empty-shell.component.html',
  styleUrls: ['./empty-shell.component.scss']
})
export class EmptyShellComponent implements OnInit {
  @Input() topRowHeight: number = 50;
  public midRowHeight: number = window.innerHeight - 150;
  @Input() bottomRowHeight: number = 40;
  @Input() sideNavWidth: number = 200;
  @Input() mainContentScroll: string = "auto"

  constructor(private falibrary: FaIconLibrary) {
    falibrary.addIcons(faCaretSquareLeft, faCaretSquareRight);
  }

  ngOnInit() {}
  ngAfterViewInit() {
    this.resetSizeValues();
  }
  
  onResize(event) {
    this.resetSizeValues();
  }

  private resetSizeValues() {
    let usedHeight = this.topRowHeight + this.bottomRowHeight;    
    let newMidHeight = window.innerHeight - usedHeight;    
    this.midRowHeight = newMidHeight-7;
  }

  public closeSideNav(){
    if (this.sideNavWidth===0){
      this.sideNavWidth = 200;
    }
    else
    {
      this.sideNavWidth = 0;
    }
  }
  
}