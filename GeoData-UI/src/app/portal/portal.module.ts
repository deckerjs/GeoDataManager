import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { PortalPage } from './portal.page';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  {
    path: '',
    loadChildren: './portal-home/portal-home.module#PortalHomePageModule'
  },
  { path: 'home', loadChildren: './portal-home/portal-home.module#PortalHomePageModule' },
  { path: 'search', loadChildren: './search/search.module#SearchPageModule' },
  { path: 'auth', loadChildren: './auth/auth.module#AuthPageModule' }, 
  { 
    path: 'geo-data-manager', 
    loadChildren: './geo-data-manager/geo-data-manager.module#GeoDataManagerPageModule',
    canLoad: [AuthGuard]
  },
  { path: 'portal-settings', loadChildren: './portal-settings/portal-settings.module#PortalSettingsPageModule' },  
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [PortalPage]
  ,exports:[
    PortalPage
  ]
})
export class PortalPageModule {}
