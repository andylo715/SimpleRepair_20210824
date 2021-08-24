import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RepairComponent } from './repair.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Repair'
    },
    children: [
      {
        path: '',
        redirectTo: 'repair'
      },
      {
        path: 'repair',
        component: RepairComponent,
        data: {
          title: 'Repair'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RepairRoutingModule { }
