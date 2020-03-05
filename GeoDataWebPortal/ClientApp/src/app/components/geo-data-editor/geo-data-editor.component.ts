import { Component, OnInit, Input } from "@angular/core";
import { GeoDataset } from "../../models/geo-dataset";
import { Guid } from "../../utilities/common-utilities";
import { GeoDataMessageBusService, MessageType } from "../../services/geo-data-message-bus.service";
import { GeoDataAPIService } from 'src/app/services/geo-data-api.service';
import { debounce, debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-geo-data-editor',
  templateUrl: './geo-data-editor.component.html',
  styleUrls: ['./geo-data-editor.component.scss']
})
export class GeoDataEditorComponent implements OnInit {

  editorOptions = { theme: "vs-dark", language: "json" };

  private _rawText: string;
  get rawText(): string {
    return this._rawText;
  }
  @Input()
  set rawText(text: string) {
    this._rawText = text;
    this.parseGeoJson();
  }

  public data: GeoDataset = new GeoDataset();
  public problems: string;

  constructor(
    private dataService: GeoDataAPIService,
    private msgService: GeoDataMessageBusService
  ) { }

  ngOnInit() {
    this.msgService.subscribeGeoDatasetSelected().pipe(debounceTime(500)).subscribe(x => {
      this.data = x;
      this._rawText = JSON.stringify(x);
      this.editorAutoFormat();
    });
  }

  public parseGeoJson() {
    if (this.rawText != null) {
      this.initNewDataset();

      try {
        let parsedData = <GeoDataset>(
          JSON.parse(this.rawText)
        );

        if (parsedData) {
          this.data = parsedData;
          this.problems = "";
        } else {
          this.data.Data = null;
          this.problems = "Could not cast data into GeoJSON object.";
        }
      } catch (e) {
        this.problems = "JSON Parse problems: " + e;
      }
    }

    console.log(this.data);
  }

  public update() {
    try {
      if (this.data != null) {
        this.dataService.update(this.data).subscribe(x => {
          next: {
            this.msgService.publishGeneral(MessageType.NewGeoDataAvailable, null);
          }
        });
      }
    } catch (e) {
      this.problems = "API problems: " + e;
    }
  }

  public addNew() {
    try {
      if (this.data != null) {
        this.dataService.create(this.data);
      }
      this.msgService.publishGeneral(MessageType.NewGeoDataAvailable, null);
    } catch (e) {
      this.problems = "API problems: " + e;
    }
  }

  public reset() {
    this.rawText = "";
    this.initNewDataset();
  }

  private initNewDataset() {
    this.data = new GeoDataset();
    this.data.Description = "Un-Named Feature";
    this.data.ID = Guid.newGuid();
  }

  private monacoEditor: any;

  private editorAutoFormat() {
    console.log('monico editor:', this.monacoEditor)
    if (this.monacoEditor) {
      setTimeout(() => { 
        this.monacoEditor.trigger('bla','editor.action.formatDocument','bla'); 
      }, 200);
    } else {
    }
  }

  public onEditorInit(editor: any) {
    this.monacoEditor = editor;
  }

}


