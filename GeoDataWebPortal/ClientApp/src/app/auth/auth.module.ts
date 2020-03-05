import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

// import { IonicModule } from '@ionic/angular';

import { AuthComponent } from './auth.component';

const routes: Routes = [
  {
    path: '',
    component: AuthComponent
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    // IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [AuthComponent]
})
export class AuthPageModule {}
