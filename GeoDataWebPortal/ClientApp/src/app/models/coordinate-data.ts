export class CoordinateData {
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
    Quality: number;
    Heading: number;
    FeetPerSecond: number;
    SatellitesInView: number
    SignalToNoiseRatio: number;
    RtkAge: number;
    RtkRatio: number;
    Hdop: number;
    EastProjectionOfBaseLine: number;
    NorthProjectionOfBaseLine: number;
    UpProjectionOfBaseLine: number;
}

export interface KeyValuePair {
    [key: string]: string;
}