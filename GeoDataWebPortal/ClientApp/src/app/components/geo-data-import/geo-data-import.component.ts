import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { map, catchError, flatMap, mergeMap, concatMap } from 'rxjs/operators';
import { HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { of, Observable, from, Subject, queueScheduler, asyncScheduler } from 'rxjs';
import { CoordinateDataMessageBusService, MessageType } from 'src/app/services/coordinate-data-message-bus.service';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faUpload, faFile, faTrash } from '@fortawesome/free-solid-svg-icons';
import { GpxImportDataService } from 'src/app/services/gpx-import-data.service';

interface FileData {
  name: string;
  size: number;
}

class ImportFile {
  constructor(data: FileData) {
    this.data = data;
  }
  data: FileData;
  inProgress: boolean;
  progress: number;
}

@Component({
  selector: 'app-geo-data-import',
  templateUrl: './geo-data-import.component.html',
  styleUrls: ['./geo-data-import.component.scss']
})
export class GeoDataImportComponent implements OnInit {

  public editorOptions = { theme: "vs-dark", language: "json" };
  private monacoEditor: any;

  constructor(
    private dataService: GpxImportDataService,
    private msgService: CoordinateDataMessageBusService, private falibrary: FaIconLibrary) {
    falibrary.addIcons(faUpload, faFile, faTrash);
  }

  ngOnInit() {
    this.editorAutoFormat();
  }

  @ViewChild("fileOpenDialog") fileOpenDialog: ElementRef;
  public files: ImportFile[] = [];
  public selectedFile: ImportFile;
  public selectedFileContent: string;

  public onFileOpenDialogClick(): void {
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
    this.uploadFiles(this.files);
  }

  public fileSelected(file: any): void {
    this.selectedFile = file;
    this.readFile(file.data);
  }

  public removeFile(file: ImportFile): void {
    this.files = this.files.filter(item => item.data.name !== file.data.name);
  }

  private readFile(file: any): void {
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      this.selectedFileContent = fileReader.result.toString();
    }
    fileReader.readAsText(file);
  }

  private readFileAsString(file: any): Observable<string> {

    let obs = Observable.create((observer: any) => {
      let fileReader = new FileReader();
      fileReader.onload = (e) => {
        observer.next(fileReader.result.toString());
        observer.complete();
      }
      fileReader.readAsText(file.data);
    });

    return obs;
  }

  private uploadFiles(files: ImportFile[]) {
    from(files, asyncScheduler).pipe(mergeMap(x => this.uploadFile(x)))
      .subscribe(
        {
          next:
            (x: any) => {
              this.msgService.publishGeneral(MessageType.NewGeoDataAvailable, null);
            }
        });
  }

  private uploadFile(file: ImportFile): Observable<any> {
    return this.readFileAsString(file).pipe(flatMap((fileContent) => {
      return this.dataService.gpxUpload(fileContent)
        .pipe(map(event => {
          if (event != null) {
            switch (event.type) {
              case HttpEventType.UploadProgress:
                file.progress = Math.round(event.loaded * 100 / event.total);
                break;
              case HttpEventType.Response:
                return event;
            }
          }
        }),
          catchError((error: HttpErrorResponse) => {
            console.log('gpxupload error:', error)
            file.inProgress = false;
            return of(`${file.data.name} upload failed.`);
          }));
    }));

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
