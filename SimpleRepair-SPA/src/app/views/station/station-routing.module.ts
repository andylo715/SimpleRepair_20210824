import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StationComponent } from './station-list/station.component';
import { StationAddComponent } from './station-add/station-add.component';
// import { StationEditComponent } from './station-edit/station-edit.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Station'
    },
    children: [
      {
        path: '',
        redirectTo: 'station'
      },
      {
        path: 'station',
        component: StationComponent,
        data: {
          title: 'Station'
        }
      },
      {
        path: "add",
        component: StationAddComponent,
        data: {
          title: "Add Station",
        },
      },
      {
        path: "edit",
        component: StationAddComponent,
        data: {
          title: "Edit Station",
        },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StationRoutingModule { }
