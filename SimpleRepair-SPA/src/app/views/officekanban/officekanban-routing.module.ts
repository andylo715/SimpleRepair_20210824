import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { OfficekanbanComponent } from './officekanban.component';

const routes: Routes = [

  {
    path: '',
    data: {
      title: 'Kanban'
    },
    children: [
      {
        path: '',
        redirectTo: 'officekanban'
      },
      {
        path: 'officekanban',
        component: OfficekanbanComponent,
        data: {
          title: 'Kanban'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OfficekanbanRoutingModule { }
