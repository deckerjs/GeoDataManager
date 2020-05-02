import { Component, OnInit, Input } from "@angular/core";
import { Guid } from "../../utilities/common-utilities";
import { CoordinateDataMessageBusService, MessageType } from "../../services/coordinate-data-message-bus.service";
import { debounce, debounceTime, delay } from 'rxjs/operators';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faUpload, faTrash, faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { CoordinateDataAPIService } from 'src/app/services/coordinate-data-api.service';
import { CoordinateData } from 'src/app/models/coordinate-data';

@Component({
  selector: 'app-geo-data-editor',
  templateUrl: './geo-data-editor.component.html',
  styleUrls: ['./geo-data-editor.component.scss']
})
export class GeoDataEditorComponent implements OnInit {

  public editorOptions = { theme: "vs-dark", language: "json" };
  private monacoEditor: any;

  private _rawText: string;
  get rawText(): string {
    return this._rawText;
  }
  @Input()
  set rawText(text: string) {
    this._rawText = text;
    this.parseGeoJson();
  }

  public data: CoordinateData = new CoordinateData();
  public problems: string;

  constructor(
    private dataService: CoordinateDataAPIService,
    private msgService: CoordinateDataMessageBusService,
    private falibrary: FaIconLibrary) {
      falibrary.addIcons(faUpload, faTrash, faPlusSquare); }

  ngOnInit() {
    this.msgService.subscribeCoordinateDatasetSelected().pipe(delay(500),debounceTime(500)).subscribe(x => {
      this.data = x;
      this._rawText = JSON.stringify(x);
      this.editorAutoFormat();
    });
  }

  public parseGeoJson() {
    if (this.rawText != null) {
      this.initNewDataset();

      try {
        let parsedData = <CoordinateData>(
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

  public clear() {
    this.rawText = "";
    this.initNewDataset();
  }

  public delete() {
    try {
      if (this.data != null) {
        this.dataService.delete(this.data.ID).subscribe(x => {
          next: {
            this.msgService.publishGeneral(MessageType.NewGeoDataAvailable, null);
            this.clear();
          }
        });
      }
    } catch (e) {
      this.problems = "API problems: " + e;
    }
  }

  private initNewDataset() {
    this.data = new CoordinateData();
    this.data.Description = "";
    this.data.ID = Guid.newGuid();
  }

  private editorAutoFormat() {
    console.log('monico editor:', this.monacoEditor)
    if (this.monacoEditor) {
      setTimeout(() => {
        this.monacoEditor.trigger('bla', 'editor.action.formatDocument', 'bla');
      }, 200);
    } else {
    }
  }

  public onEditorInit(editor: any) {
    this.monacoEditor = editor;
  }

}


