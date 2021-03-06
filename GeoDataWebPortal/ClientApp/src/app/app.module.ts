import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { EmptyShellComponent } from './components/empty-shell/empty-shell.component';
import { EmptyModuleShellComponent } from './components/empty-module-shell/empty-module-shell.component';
import { GeoDataImportComponent } from './components/geo-data-import/geo-data-import.component';
import { GeoDataManagerComponent } from './components/geo-data-manager/geo-data-manager.component';
import { GeoDataSelectorComponent } from './components/geo-data-selector/geo-data-selector.component';
import { GmapViewComponent } from './components/gmap-view/gmap-view.component';
import { MainViewComponent } from './components/main-view/main-view.component';
import { CoordinateDataMessageBusService } from './services/coordinate-data-message-bus.service';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { FormsModule } from '@angular/forms';
import { GeoDataEditorComponent } from './components/geo-data-editor/geo-data-editor.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthComponent } from './auth/auth.component';
import { GeoDataViewerComponent } from './components/geo-data-viewer/geo-data-viewer.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DataSettingsComponent } from './components/data-settings/data-settings.component';
import { GpsTelemetryChartComponent } from './components/gps-telemetry-chart/gps-telemetry-chart.component';
import { ChartsModule } from 'ng2-charts';
import { ImageDataImportComponent } from './components/image-data-import/image-data-import.component';
import { CoordinateDataSummaryItemComponent } from './components/coordinate-data-summary-item/coordinate-data-summary-item.component';
import { GeoDataCollectComponent } from './components/geo-data-collect/geo-data-collect.component';

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
    GeoDataViewerComponent,
    DataSettingsComponent,
    GpsTelemetryChartComponent,
    ImageDataImportComponent,
    CoordinateDataSummaryItemComponent,
    GeoDataCollectComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,    
    MonacoEditorModule.forRoot(),
    HttpClientModule,
    NgbModule,
    FontAwesomeModule,
    ChartsModule    
  ],
  providers: [CoordinateDataMessageBusService],
  bootstrap: [AppComponent]
})
export class AppModule {}
