import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

// import { AgmCoreModule } from '@agm/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { EmptyShellComponent } from './components/empty-shell/empty-shell.component';
import { EmptyModuleShellComponent } from './components/empty-module-shell/empty-module-shell.component';
import { GeoDataImportComponent } from './components/geo-data-import/geo-data-import.component';
import { GeoDataManagerComponent } from './components/geo-data-manager/geo-data-manager.component';
import { GeoDataSelectorComponent } from './components/geo-data-selector/geo-data-selector.component';
import { GmapViewComponent } from './components/gmap-view/gmap-view.component';
import { MainViewComponent } from './components/main-view/main-view.component';
import { GeoDataLocalRepositoryService } from './services/geo-data-local-repository.service';
import { GeoDataMessageBusService } from './services/geo-data-message-bus.service';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { FormsModule } from '@angular/forms';
import { GeoDataEditorComponent } from './components/geo-data-editor/geo-data-editor.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthComponent } from './auth/auth.component';
import { GeoDataViewerComponent } from './components/geo-data-viewer/geo-data-viewer.component';


@NgModule({
  declarations: [
    AppComponent,
    EmptyShellComponent,
    EmptyModuleShellComponent,
    GeoDataImportComponent,
    GeoDataManagerComponent,
    GeoDataSelectorComponent,
    GmapViewComponent,
    MainViewComponent,
    GeoDataEditorComponent,
    AuthComponent,
    GeoDataViewerComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    NgbModule,
    MonacoEditorModule.forRoot(),
    HttpClientModule
  ],
  providers: [GeoDataLocalRepositoryService, GeoDataMessageBusService],
  bootstrap: [AppComponent]
})
export class AppModule {}
