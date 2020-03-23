import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { GeoDataAPIService } from 'src/app/services/geo-data-api.service';
import { map, catchError } from 'rxjs/operators';
import { HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { of } from 'rxjs';
import { GeoDataMessageBusService, MessageType } from 'src/app/services/geo-data-message-bus.service';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faUpload, faFile } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-geo-data-import',
  templateUrl: './geo-data-import.component.html',
  styleUrls: ['./geo-data-import.component.scss']
})
export class GeoDataImportComponent implements OnInit {

  public editorOptions = { theme: "vs-dark", language: "json" };
  private monacoEditor: any;

  constructor(
    private dataService: GeoDataAPIService,
    private msgService: GeoDataMessageBusService, private falibrary: FaIconLibrary) {
    falibrary.addIcons(faUpload, faFile);
  }

  ngOnInit() {
    this.editorAutoFormat();
  }

  @ViewChild("fileOpenDialog") fileOpenDialog: ElementRef;
  public files = [];
  public selectedFile: any;
  public selectedFileContent: string;

  public onFileOpenDialogClick(): void {
    console.log('open click')
    const fileDialog = this.fileOpenDialog.nativeElement;
    fileDialog.onchange = () => {
      for (let index = 0; index < fileDialog.files.length; index++) {
        const file = fileDialog.files[index];
        this.files.push({ data: file, inProgress: false, progress: 0 });
        this.readFile(file);
      }
    };
    fileDialog.click();
  }

  public onUploadClick(): void {
    this.uploadFile(this.selectedFile);
  }

  public fileSelected(file: any): void {
    this.selectedFile = file;
  }

  private readFile(file: any): void {
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      this.selectedFileContent = fileReader.result.toString();
    }
    fileReader.readAsText(file);
  }

  private uploadFile(file) {

    this.dataService.gpxUpload(this.selectedFileContent)
      .pipe(map(event => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            file.progress = Math.round(event.loaded * 100 / event.total);
            break;
          case HttpEventType.Response:
            return event;
        }
      }),
        catchError((error: HttpErrorResponse) => {
          console.log('gpxupload error:', error)
          file.inProgress = false;
          return of(`${file.data.name} upload failed.`);
        }))
      .subscribe((event: any) => {
        console.log('uploaded:', event);
        this.msgService.publishGeneral(MessageType.NewGeoDataAvailable, null);
      });

  }

  public formatBytes(bytes: number, decimals: number = 2): string {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  public onEditorInit(editor: any) {
    this.monacoEditor = editor;
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

}
