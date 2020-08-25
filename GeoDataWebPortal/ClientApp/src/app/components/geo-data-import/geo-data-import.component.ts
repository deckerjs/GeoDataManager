import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { map, catchError, flatMap, mergeMap, concatMap } from 'rxjs/operators';
import { HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { of, Observable, from, Subject, queueScheduler, asyncScheduler } from 'rxjs';
import { CoordinateDataMessageBusService, MessageType } from 'src/app/services/coordinate-data-message-bus.service';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faUpload, faFile, faTrash } from '@fortawesome/free-solid-svg-icons';
import { GpxImportDataService } from 'src/app/services/gpx-import-data.service';
import { ImageImportDataService } from 'src/app/services/image-import-data.service';
import { CoordinateDataAPIService } from 'src/app/services/coordinate-data-api.service';

const FILETYPE_GPX: string = 'gpx';
const FILETYPE_JPG: string = 'jpg';

interface FileInfo extends File {
  importType: string;
}

class ImportFile {
  constructor(info: FileInfo) {
    this.fileInfo = info;
  }
  fileInfo: FileInfo;
  inProgress: boolean;
  progress: number;
}

interface idSelection {
  id: string,
  description: string
}

@Component({
  selector: 'app-geo-data-import',
  templateUrl: './geo-data-import.component.html',
  styleUrls: ['./geo-data-import.component.scss']
})
export class GeoDataImportComponent implements OnInit {

  private monacoEditor: any;
  public editorOptions = { theme: "vs-dark", language: "json" };
  public idSelectOptions: Array<idSelection> = [];
  public selectedImportId: string;

  constructor(
    private gpxDataService: GpxImportDataService,
    private imageDataService: ImageImportDataService,
    private msgService: CoordinateDataMessageBusService,
    private apiDataService: CoordinateDataAPIService,
    private falibrary: FaIconLibrary) {
    falibrary.addIcons(faUpload, faFile, faTrash);
  }

  ngOnInit() {
    this.editorAutoFormat();
    this.refreshIdList();
  }

  @ViewChild("fileOpenDialog") fileOpenDialog: ElementRef;
  public importFiles: ImportFile[] = [];
  public selectedFile: ImportFile;
  public selectedFileContent: string;

  public onFileOpenDialogClick(): void {
    const fileDialog = this.fileOpenDialog.nativeElement;
    fileDialog.onchange = () => {
      for (let index = 0; index < fileDialog.files.length; index++) {

        const file = fileDialog.files[index];
        this.setImportType(file);
        this.importFiles.push({ fileInfo: file, inProgress: false, progress: 0 });

        if (file.importType === FILETYPE_GPX) {
          this.readFileText(file);
        }

        // todo: instead of text show image preview
        // if(file.type === FILETYPE_JPG){
        //   this.readFileText(file);
        // }

      }
    };
    fileDialog.click();
  }

  private setImportType(file: FileInfo): void {
    if (file && file.name) {
      const ext = file.name.split('.').pop();
      if (ext) {
        if (ext.toLowerCase() === FILETYPE_GPX) {
          file.importType = FILETYPE_GPX;
        }
        if (ext.toLowerCase() === FILETYPE_JPG) {
          file.importType = FILETYPE_JPG;
        }
      }
    }
  }

  private refreshIdList(): void {

    this.apiDataService.GetAllOwnedSummary().subscribe({
      next: x => {
        this.idSelectOptions = x.map(x => <idSelection>{
          id: x.ID, description: x.Description
        });
      }
    });
  }

  public selectId(id: string): void {
    this.selectedImportId = id;
  }

  public onUploadClick(): void {
    this.uploadFiles(this.importFiles);
  }

  public removeFile(file: ImportFile): void {
    this.importFiles = this.importFiles.filter(item => item.fileInfo.name !== file.fileInfo.name);
  }

  private readFileText(file: FileInfo): void {
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      this.selectedFileContent = fileReader.result.toString();
    }
    fileReader.readAsText(file);
  }

  private readFileAsString(file: ImportFile): Observable<string> {
    let obs = Observable.create((observer: any) => {
      let fileReader = new FileReader();
      fileReader.onload = (e) => {
        observer.next(fileReader.result.toString());
        observer.complete();
      }
      fileReader.readAsText(file.fileInfo);
    });
    return obs;
  }

  private readFileAsBlob(file: ImportFile): Observable<string> {
    let obs = Observable.create((observer: any) => {
      let fileReader = new FileReader();
      fileReader.onload = (e) => {
        const readResult = fileReader.result.toString();
        const b64str = btoa(readResult);
        observer.next(b64str);
        observer.complete();
      }
      fileReader.readAsBinaryString(file.fileInfo);
    });
    return obs;
  }

  private uploadFiles(files: ImportFile[]) {
    from(files.filter(x => x.fileInfo.importType === FILETYPE_GPX), asyncScheduler).pipe(mergeMap(x => this.uploadGpxFile(x)))
      .subscribe(
        {
          next:
            (x: any) => {
              this.msgService.publishGeneral(MessageType.NewGeoDataAvailable, null);
            }
        });

    from(files.filter(x => x.fileInfo.importType === FILETYPE_JPG), asyncScheduler).pipe(mergeMap(x => this.uploadImageFile(x)))
      .subscribe(
        {
          next:
            (x: any) => {
              this.msgService.publishGeneral(MessageType.NewGeoDataAvailable, null);
            }
        });

  }

  private uploadGpxFile(file: ImportFile): Observable<any> {
    return this.readFileAsString(file).pipe(flatMap((fileContent) => {
      return this.gpxDataService.gpxUpload(fileContent)
        .pipe(map(event => {
          this.handleUploadEvent(file, event);
        }),
          catchError((error: HttpErrorResponse) => {
            console.log('gpxupload error:', error)
            file.inProgress = false;
            return of(`${file.fileInfo.name} upload failed.`);
          }));
    }));
  }

  private uploadImageFile(file: ImportFile): Observable<any> {


    return this.readFileAsBlob(file).pipe(flatMap((fileContent) => {

      let uploadMethod: Observable<any>;
      if (this.selectedImportId) {
        uploadMethod = this.imageDataService.imageUploadToExisting(this.selectedImportId, fileContent);
      } else {
        uploadMethod = this.imageDataService.imageUploadToNew(fileContent);
      }

      return uploadMethod
        .pipe(map(event => {
          this.handleUploadEvent(file, event);
        }),
          catchError((error: HttpErrorResponse) => {
            console.log('image upload error:', error)
            file.inProgress = false;
            return of(`${file.fileInfo.name} upload failed.`);
          }));
    }));
  }

  private handleUploadEvent(file: ImportFile, event: any) {
    if (event != null) {
      switch (event.type) {
        case HttpEventType.UploadProgress:
          file.progress = Math.round(event.loaded * 100 / event.total);
          break;
        case HttpEventType.Response:
          return event;
      }
    }
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
