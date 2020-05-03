import { Component, OnInit, Input } from "@angular/core";
import { GeoDataset } from "../../models/geo-dataset";
import { geojsonHelperFunctions } from "../../utilities/geojson-helper-functions";
import { CoordinateDataMessageBusService } from "../../services/coordinate-data-message-bus.service";
import * as mapboxgl from 'mapbox-gl';
import { FeatureCollection, LineString } from 'geojson';
import { AreaBounds } from 'src/app/models/area-bounds';
import { LatLngPoint } from 'src/app/models/lat-lng-point';
import { SettingsService } from 'src/app/portal-settings/settings.service';
import { take, delay } from 'rxjs/operators';
import { GeoDataAPIService } from 'src/app/services/geo-data-api.service';

const outdoorsv9: string = 'outdoors-v9';
const outdoorsv11: string = 'outdoors-v11';
const satellitev9: string = 'satellite-v9';
const lightv10: string = 'light-v10';
const darkv10: string = 'dark-v10';
const streetsv11: string = 'streets-v11';

@Component({
  selector: "app-gmap-view",
  templateUrl: "./gmap-view.component.html",
  styleUrls: ["./gmap-view.component.scss"]
})
export class GmapViewComponent implements OnInit {

  public map: mapboxgl.Map;
  public source: any;

  private style = 'mapbox://styles/mapbox/' + lightv10;
  private lat: number;
  private lng: number;

  constructor(private msgService: CoordinateDataMessageBusService,
    private settingsService: SettingsService,
    private dataService: GeoDataAPIService) {
    settingsService.getSettings().pipe(take(1)).subscribe({
      next: config => {
        mapboxgl.accessToken = config.MapboxToken;
      }
    });
  }

  ngOnInit() {
    this.msgService.subscribeCoordinateDatasetSelected().pipe(delay(150)).subscribe(x => {
      console.log('incoming map data sub:', x)
      if (x.Data != null) {

        this.dataService.Get(x.ID).subscribe({
          next: geoData => {
            this.mapData = geoData;
            this.buildMap();
            this.map.flyTo({
              center: [this.lng, this.lat]
            });

          }
        });

      }
    });
  }

  private _mapData: GeoDataset;
  get mapData(): GeoDataset {
    return this._mapData;
  }
  @Input()
  set mapData(data: GeoDataset) {
    this._mapData = data;
  }

  private _mapHeight: string = "100em";
  get mapHeight(): string {
    return this._mapHeight;
  }
  @Input()
  set mapHeight(height: string) {
    this._mapHeight = height;
  }


  private buildMap() {
    const featureCollection = this.mapData.Data;

    let areaBounds: AreaBounds = geojsonHelperFunctions.getBounds(featureCollection);
    let center: LatLngPoint = geojsonHelperFunctions.getCenter(areaBounds);

    console.log('got bounds, center:', areaBounds, center)

    this.lat = center.lat;
    this.lng = center.lng;

    this.map = new mapboxgl.Map({
      container: 'map',
      style: this.style,
      zoom: 13
      , center: [this.lng, this.lat]
    });

    this.map.addControl(new mapboxgl.NavigationControl());

    this.map.on('load', event => {
      this.map.addSource('stuff', {
        type: 'geojson',
        data: {
          type: 'FeatureCollection',
          features: []
        }
      });

      this.source = this.map.getSource('stuff');
      this.source.setData(featureCollection);

      this.map.addLayer({
        id: 'somestuff',
        source: 'stuff',
        type: 'line',
        layout: {
          'line-join': 'round',
          'line-cap': 'round'
        },
        paint: {
          'line-color': '#888',
          'line-width': 4
        }
      });

      const bounds = this.getBoundsFromFeatureCollection(featureCollection);
      this.map.fitBounds(bounds, {
        padding: 20
      });
    });
  }

  private getBoundsFromFeatureCollection(featureCollection: FeatureCollection): mapboxgl.LngLatBounds {
    const bounds = new mapboxgl.LngLatBounds();

    featureCollection.features.forEach(function (f) {
      const fline = <LineString>f.geometry;
      if (fline && fline.coordinates) {
        let fbound = new mapboxgl.LngLatBounds();
        fline.coordinates.forEach(c => {
          fbound = new mapboxgl.LngLatBounds(c, c);
          bounds.extend(fbound);
        });
      }
    });

    return bounds;
  }

}
