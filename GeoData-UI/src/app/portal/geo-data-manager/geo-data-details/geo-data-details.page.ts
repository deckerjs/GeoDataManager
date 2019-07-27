import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GeoData } from '../models/geo-data';
import { GeoDataAPIService } from '../services/geo-data-api.service';
import { NavController } from '@ionic/angular';

@Component({
	selector: 'app-geo-data-details',
	templateUrl: './geo-data-details.page.html',
	styleUrls: ['./geo-data-details.page.scss']
})
export class GeoDataDetailsPage implements OnInit {
	public geoDataSet: GeoData;

	constructor(
		private route: ActivatedRoute,
		private dataService: GeoDataAPIService,
		private navCtrl: NavController
	) {}

	ngOnInit() {
		this.route.paramMap.subscribe(paramMap => {
			if (!paramMap.has('id')) {
				this.navCtrl.navigateBack('/geo-data-mamanger');
				return;
			}
			this.dataService.Get(paramMap.get('id')).subscribe(data => {
				this.geoDataSet = data;
			});
		});
	}
}
