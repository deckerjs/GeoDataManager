import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { GeoDataManagerPage } from './geo-data-manager.page';

const routes: Routes = [
	{
		path: '',
		component: GeoDataManagerPage
	},
	{
		path: 'map-view/:id',
		loadChildren: './map-view/map-view.module#MapViewPageModule'
	},
	{
		path: 'data-query',
		loadChildren: './data-query/data-query.module#DataQueryPageModule'
	},
	{
		path: 'data-import',
		loadChildren: './data-import/data-import.module#DataImportPageModule'
	},
	{
		path: 'geo-data-details/:id',
		loadChildren:
			'./geo-data-details/geo-data-details.module#GeoDataDetailsPageModule'
	}
];

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		IonicModule,
		RouterModule.forChild(routes)
	],
	declarations: [GeoDataManagerPage]
})
export class GeoDataManagerPageModule {}
