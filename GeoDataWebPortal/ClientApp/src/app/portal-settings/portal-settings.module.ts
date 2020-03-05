import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { PortalSettingsPage } from './portal-settings.page';

const routes: Routes = [
  {
    path: '',
    component: PortalSettingsPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes)
  ],
  declarations: [PortalSettingsPage]
})
export class PortalSettingsPageModule {}
