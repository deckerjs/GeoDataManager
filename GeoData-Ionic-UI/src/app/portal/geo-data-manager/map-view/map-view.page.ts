import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GeoDataAPIService } from '../services/geo-data-api.service';
import { GeoData } from '../models/geo-data';
import { NavController } from '@ionic/angular';
import * as mapboxgl from 'mapbox-gl';
import { environment } from 'src/environments/environment';
import { LineString, FeatureCollection } from 'geojson';

@Component({
	selector: 'app-map-view',
	templateUrl: './map-view.page.html',
	styleUrls: ['./map-view.page.scss']
})
export class MapViewPage implements OnInit {
	public geoDataSet: GeoData;

	public map: mapboxgl.Map;
	// outdoors-v9
	// outdoors-v11
	// satellite-v9
	// light-v10
	// dark-v10
	// streets-v11
	private style = 'mapbox://styles/mapbox/light-v10';
	private lat = 37.75;
	private lng = -122.41;
	private message = 'Hello World!';
	public source: any;

	constructor(
		private route: ActivatedRoute,
		private dataService: GeoDataAPIService,
		private navCtrl: NavController
	) {
		mapboxgl.accessToken = environment.mapboxToken;
	}

	ngOnInit() {
		this.route.paramMap.subscribe(paramMap => {
			if (!paramMap.has('id')) {
				this.navCtrl.navigateBack('/geo-data-mamanger');
				return;
			}
			this.dataService.Get(paramMap.get('id')).subscribe(data => {
				console.log('got geodata in map:', data);
				this.geoDataSet = data;
				this.initializeMap();
			});
		});
	}

	private initializeMap() {
		if (navigator.geolocation) {
			navigator.geolocation.getCurrentPosition(position => {
				this.lat = position.coords.latitude;
				this.lng = position.coords.longitude;
				this.map.flyTo({
					center: [this.lng, this.lat]
				});
			});
		}

		this.buildMap();
	}

	private buildMap() {
		this.map = new mapboxgl.Map({
			container: 'map',
			style: this.style,
			zoom: 13,
			center: [this.lng, this.lat]
		});

		/// Add map controls
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
			this.source.setData(this.geoDataSet.Data);

			this.map.addLayer({
				id: 'somestuff',
				source: 'stuff',
				type: 'line'
			});

			const bounds = this.getBoundsFromFeatureCollection(this.geoDataSet.Data);

			this.map.fitBounds(bounds, {
				padding: 20
			});
		});
	}

	private getBoundsFromFeatureCollection(featureCollection: FeatureCollection): mapboxgl.LngLatBounds {
		const bounds = new mapboxgl.LngLatBounds();

		featureCollection.features.forEach(function(f) {
			const fline = <LineString>f.geometry;
			if (fline && fline.coordinates) {
				let fbound = new mapboxgl.LngLatBounds();
				fline.coordinates.forEach(c => {
					fbound = new mapboxgl.LngLatBounds(c, c);
					bounds.extend(fbound);
				});
				console.log('bounds', bounds);
			}
		});

		return bounds;
	}
}
