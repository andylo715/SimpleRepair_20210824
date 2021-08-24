import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AlertModule } from "ngx-bootstrap/alert";
import { PaginationModule } from "ngx-bootstrap/pagination";

import { OfficekanbanRoutingModule } from './officekanban-routing.module';
import { OfficekanbanComponent } from './officekanban.component';


@NgModule({
  declarations: [OfficekanbanComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AlertModule.forRoot(),
    PaginationModule,
    OfficekanbanRoutingModule
  ]
})
export class OfficekanbanModule { }
