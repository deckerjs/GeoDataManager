import { Injectable } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { filter } from "rxjs/operators";
import { GeoDataset } from "../models/geo-dataset";

@Injectable({
  providedIn: "root"
})
export class GeoDataMessageBusService {
  private msgSubject: BehaviorSubject<MessageData>;
  private geoDatasetSelectedSubject: BehaviorSubject<GeoDataset>;

  constructor() {
    this.msgSubject = new BehaviorSubject<MessageData>(new MessageData());
    this.geoDatasetSelectedSubject = new BehaviorSubject<GeoDataset>(new GeoDataset());
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
