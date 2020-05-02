import { Injectable } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { filter, debounceTime } from "rxjs/operators";
import { CoordinateData } from '../models/coordinate-data';

@Injectable({
  providedIn: "root"
})
export class CoordinateDataMessageBusService {
  private msgSubject: BehaviorSubject<MessageData>;
  private coordinateDataSelectedSubject: BehaviorSubject<CoordinateData>;

  constructor() {
    this.msgSubject = new BehaviorSubject<MessageData>(new MessageData());
    this.coordinateDataSelectedSubject = new BehaviorSubject<CoordinateData>(new CoordinateData());
  }

  public subscribeCoordinateDatasetSelected(): Observable<CoordinateData> {
    return this.coordinateDataSelectedSubject;
  }

  public subscribeGeneral(type: MessageType): Observable<MessageData> {
    return this.msgSubject.pipe(filter(x => x.messageType === type), debounceTime(500));
  }

  public publishCoordinateDatasetSelected(data: CoordinateData) {
    console.log('CoordinateData published:');
    console.log(data);
    this.coordinateDataSelectedSubject.next(data);
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