import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RepairRoutingModule } from './repair-routing.module';
import { RepairComponent } from './repair.component';


@NgModule({
  declarations: [RepairComponent],
  imports: [
    CommonModule,
    RepairRoutingModule
  ]
})
export class RepairModule { }
