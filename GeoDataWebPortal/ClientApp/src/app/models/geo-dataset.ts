export class GeoDataset {
  constructor() {}

  public ID: string;
  public UserID:string;
  public Description:string;
  public DateCreated:Date;
  public DateModified:Date;
  public Tags:Array<string>;
  public Data: GeoJSON.FeatureCollection<any>;
}

export class GeoDataSummary {
  constructor() {}
    public ID: string;
    public Name:string;
    public Items:number;
}


