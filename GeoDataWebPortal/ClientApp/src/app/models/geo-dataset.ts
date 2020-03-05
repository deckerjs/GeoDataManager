export class GeoDataset {
  constructor() {}

  public ID: string;
  public Description:string;
  public Data: GeoJSON.FeatureCollection<any>;
}

export class GeoDataSummary {
  constructor() {}
    public ID: string;
    public Name:string;
    public Items:number;
}


