import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GeoDataManagerComponent } from './components/geo-data-manager/geo-data-manager.component';

const routes: Routes = [
  {path: '', redirectTo: '/data-manager', pathMatch: 'full' },
  {path:'data-manager', component: GeoDataManagerComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
