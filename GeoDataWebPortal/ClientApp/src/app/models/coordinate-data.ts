export class CoordinateData implements CoordinateDataInfo{
    ID: string;
    UserID: string;
    Description: string;
    DateModified: Date;
    DateCreated: Date;
    Tags: Array<string>;
    Data: Array<PointCollection>
}

export interface PointCollection {
    ID: string
    Metadata: Array<KeyValuePair>;
    Coordinates: Array<Coordinate>;
}

export interface Coordinate {
    Latitude: number;
    Longitude: number;
    Altitude: number;
    Time: Date;
    Telemetry: GpsTelemetry;
}


export interface GpsTelemetry {
    [key: string]: number;
}

export interface KeyValuePair {
    [key: string]: string;
}

export interface CoordinateDataSummary extends CoordinateDataInfo{
    DataSegmentCount:number;
    DataItemCount:number;
    SummaryData:KeyValuePair;
}

export interface CoordinateDataInfo{
    ID: string;
    UserID: string;
    Description: string;
    DateModified: Date;
    DateCreated: Date;
    Tags: Array<string>;
}
