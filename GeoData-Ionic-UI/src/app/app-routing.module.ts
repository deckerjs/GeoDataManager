import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'portal',
    pathMatch: 'full'
  },

  { 
    path: 'portal', 
    loadChildren: './portal/portal.module#PortalPageModule' 
  },   
  { 
    path: 'portal-home', 
    loadChildren: './portal/portal-home/portal-home.module#PortalHomePageModule' 
  } 
  
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
