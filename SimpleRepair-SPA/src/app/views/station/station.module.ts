import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AlertModule } from "ngx-bootstrap/alert";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { NgSelect2Module } from 'ng-select2';

import { StationRoutingModule } from './station-routing.module';
import { StationComponent } from './station-list/station.component';
import { StationAddComponent } from './station-add/station-add.component';
import { StationEditComponent } from './station-edit/station-edit.component';

@NgModule({
  declarations: [StationComponent, StationAddComponent, StationEditComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AlertModule.forRoot(),
    PaginationModule,
    NgSelect2Module,
    StationRoutingModule
  ]
})
export class StationModule { }
