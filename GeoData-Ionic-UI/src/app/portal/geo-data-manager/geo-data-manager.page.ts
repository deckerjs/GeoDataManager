import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { GeoDataAPIService } from './services/geo-data-api.service';
import { GeoData, itemStatus } from './models/geo-data';
import { Router } from '@angular/router';
import { Plugins, FilesystemDirectory, FilesystemEncoding } from '@capacitor/core';
import { FeatureCollection } from 'geojson';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

const { Filesystem } = Plugins;

@Component({
	selector: 'app-geo-data-manager',
	templateUrl: './geo-data-manager.page.html',
	styleUrls: ['./geo-data-manager.page.scss']
})
export class GeoDataManagerPage implements OnInit {
	private MAP_VIEW_ROUTE = '/geo-data-manager/map-view';
	private GEO_DATA_DETAILS_ROUTE = '/geo-data-manager/geo-data-details';

	@ViewChild('filePicker') filePickerRef: ElementRef<HTMLInputElement>;



	constructor(
		private router: Router,
		private dataService: GeoDataAPIService) { }

	public geoDataSets: Array<GeoData> = [];

	ngOnInit() {
		this.loadDatasets().subscribe(x => {
			console.log("finished getting data:", x)
		});

	}

	private loadDatasets(): Observable<any> {
		return this.dataService.GetAll().pipe(tap(data => {
			this.geoDataSets = data;
			console.log('got datasets', this.geoDataSets);
		}));
	}

	public openMapView(id: string): void {
		console.log('open map:', id);
		this.router.navigate([this.MAP_VIEW_ROUTE, id]);
	}

	public openGeoDataDetails(id: string): void {
		this.router.navigate([this.GEO_DATA_DETAILS_ROUTE, id]);
	}

	public deleteGeoData(id: string): void {
		this.dataService.delete(id).subscribe({next: x=>{
			this.loadDatasets().subscribe();
		}})
	}

	public openFile(): void {
		//bit of a hack
		this.filePickerRef.nativeElement.click();
	}

	public fileSelected(event: any) {
		console.log("file selected", event);

		const pickedFile = (event.target as HTMLInputElement).files[0];
		if (!pickedFile) {
			return;
		}
		const fr = new FileReader();
		fr.onload = () => {
			const dataFromfile = fr.result.toString();

			console.log("got file contents: ", dataFromfile)
			console.log("picked file: ", pickedFile)
			const newGeoData: GeoData = {
				ID: null,
				Description: "new from file",
				Data: <FeatureCollection>JSON.parse(dataFromfile),
				UserID: null,
				DateCreated: new Date,
				DateModified: new Date,
				Tags: [],
				status: itemStatus.new
			}

			this.dataService.create(newGeoData).subscribe({
				next: n => {
					console.log('created: ', n);
					this.loadDatasets().subscribe();
				},
				error: e => {
					console.log('Error during create: ', e)
				}
			});
		};
		fr.readAsText(pickedFile);
	}

}
