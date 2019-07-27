import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { GeoDataDetailsPage } from './geo-data-details.page';

const routes: Routes = [
  {
    path: '',
    component: GeoDataDetailsPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [GeoDataDetailsPage]
})
export class GeoDataDetailsPageModule {}
