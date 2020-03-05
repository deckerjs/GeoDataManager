import { Injectable } from "@angular/core";
import { GeoDataset, GeoDataSummary } from "../models/geo-dataset";
import { Observable, ObjectUnsubscribedError } from "rxjs";
import { from } from "rxjs";
import { GeoDataDexieDB } from "./geo-data-dexie-db";
//import { fromPromise } from "rxjs/observable/fromPromise";
import { Guid } from "../utilities/common-utilities";

@Injectable({
  providedIn: "root"
})

//create data providers that conform to an interface so you can switch between api, elastic, firebase ect
export class GeoDataLocalRepositoryService {
  private locaGeoDataDB: GeoDataDexieDB;

  constructor() {
    this.locaGeoDataDB = new GeoDataDexieDB();
  }

  // public getDatasetSummary(): Observable<Array<GeoDataSummary>>{
  // return ;
  // }

  // public getDatasetsFiltered(criteria:string):Observable<Array<GeoDataset>>{
  // return ;
  // }

  public getAllDatasets(): Observable<Array<GeoDataset>> {
    return from(this.locaGeoDataDB.geoDataSets.toArray());
  }

  public getDataset(ID: string): Observable<GeoDataset> {
    return from(this.locaGeoDataDB.geoDataSets.where('ID').equals(ID).first());
  }

  public createDataset(data: GeoDataset): Observable<any> {    
    data.ID = Guid.newGuid();
    return from(this.locaGeoDataDB.geoDataSets.add(data));
  }

  public updateDataset(data: GeoDataset): Observable<any> {
    return from(this.locaGeoDataDB.geoDataSets.update(data.ID,data));
  }

  public deleteDataset(data: GeoDataset): Observable<any> {
    return from(this.locaGeoDataDB.geoDataSets.delete(data.ID));
  }
}
