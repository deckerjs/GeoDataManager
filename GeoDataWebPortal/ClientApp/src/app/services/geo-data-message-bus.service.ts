import { Injectable } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { filter } from "rxjs/operators";
import { GeoDataset } from "../models/geo-dataset";

@Injectable({
  providedIn: "root"
})
export class GeoDataMessageBusService {
  private msgSubject: Subject<MessageData>;
  private geoDatasetSelectedSubject: Subject<GeoDataset>;

  constructor() {
    this.msgSubject = new Subject<MessageData>();
    this.geoDatasetSelectedSubject = new Subject<GeoDataset>();
  }

  public subscribeGeoDatasetSelected(): Observable<GeoDataset> {
    return this.geoDatasetSelectedSubject;
  }

  public subscribeGeneral(type: MessageType): Observable<MessageData> {
    return this.msgSubject.pipe(filter(x => x.messageType === type));
  }

  public publishGeoDatasetSelected(data: GeoDataset) {
    console.log('geodataset published:');
    console.log(data);
    this.geoDatasetSelectedSubject.next(data);
  }

  public publishGeneral(type: MessageType, data: any) {
    let msgData: MessageData = new MessageData();
    msgData.messageType = type;
    msgData.data = data;
    this.msgSubject.next(msgData);
  }
}

export class MessageData {
  messageType: MessageType;
  data: any;
}

export enum MessageType {
  NewGeoDataAvailable,
  LoginStateChange
}
