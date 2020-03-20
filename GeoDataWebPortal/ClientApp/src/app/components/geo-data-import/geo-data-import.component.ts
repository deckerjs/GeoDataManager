import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-geo-data-import',
  templateUrl: './geo-data-import.component.html',
  styleUrls: ['./geo-data-import.component.scss']
})
export class GeoDataImportComponent implements OnInit {

  public editorOptions = { theme: "vs-dark", language: "json" };
  private monacoEditor: any;

  constructor() { }

  ngOnInit() {
    this.editorAutoFormat();
  }

  @ViewChild("fileUpload", { static: false }) fileUpload: ElementRef;
  public files = [];
  public selectedFile: string;

  onClick() {
    const fileUpload = this.fileUpload.nativeElement; fileUpload.onchange = () => {
      for (let index = 0; index < fileUpload.files.length; index++) {
        const file = fileUpload.files[index];
        this.files.push({ data: file, inProgress: false, progress: 0 });
        console.log('************************ files array:', this.files);

        this.selectFile(file);

      }
      this.uploadFiles();
    };
    fileUpload.click();
  }

  private selectFile(file: any): void {
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      this.selectedFile = fileReader.result.toString();
    }
    fileReader.readAsText(file);
  }

  private uploadFiles() {
    this.fileUpload.nativeElement.value = '';
    this.files.forEach(file => {
      this.uploadFile(file);
    });
  }

  uploadFile(file) {
    const formData = new FormData();
    formData.append('file', file.data);
    file.inProgress = true;
    console.log('file:', file)

    // this.uploadService.upload(formData).pipe(
    //   map(event => {
    //     switch (event.type) {
    //       case HttpEventType.UploadProgress:
    //         file.progress = Math.round(event.loaded * 100 / event.total);
    //         break;
    //       case HttpEventType.Response:
    //         return event;
    //     }
    //   }),
    //   catchError((error: HttpErrorResponse) => {
    //     file.inProgress = false;
    //     return of(`${file.data.name} upload failed.`);
    //   })).subscribe((event: any) => {
    //     if (typeof (event) === 'object') {
    //       console.log(event.body);
    //     }
    //   });
  }


  public onEditorInit(editor: any) {
    this.monacoEditor = editor;
  }

  private editorAutoFormat() {
    console.log('monico editor:', this.monacoEditor)
    if (this.monacoEditor) {
      setTimeout(() => { 
        this.monacoEditor.trigger('bla','editor.action.formatDocument','bla'); 
      }, 200);
    } else {
    }
  }

}
