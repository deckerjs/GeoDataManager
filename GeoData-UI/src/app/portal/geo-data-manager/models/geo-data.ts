import { FeatureCollection } from 'geojson';

export interface GeoData extends ItemState {
	ID: string;
	UserID: string;
	Description: string;
	DateCreated: Date;
	DateModified: Date;
	Tags: Array<string>;
	Data: FeatureCollection;	
}

export interface ItemState{
	status: itemStatus;
}

export enum itemStatus{
	clean,
	new,
	dirty
}
