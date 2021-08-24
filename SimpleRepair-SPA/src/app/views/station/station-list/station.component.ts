import { navItems } from './../../../_nav';
import { Component, OnInit } from '@angular/core';

import {
  PaginatedResult,
  Pagination,
} from "../../../_core/_models/pagination";

import { LineStation } from "../../../_core/_models/LineStation";
import { StationService } from "../../../_core/_services/Station.service";
import { Router } from "@angular/router";
import { AlertifyService } from "../../../_core/_services/alertify.service";
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-station',
  templateUrl: './station.component.html',
  styleUrls: ['./station.component.scss']
})
export class StationComponent implements OnInit {
  linestations: LineStation[]; // model container
  linestation: any = {}; // add function容器
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  paramSearch: string; // API helper(query condition)

  constructor(
    private lineStationService: StationService,
    private alertify: AlertifyService,
    private router: Router,
    private spinner: NgxSpinnerService,
  ) { }

  ngOnInit() {
    this.spinner.show();
    this.paramSearch = "";
    this.loadStation();
    this.spinner.hide();
  }

  // get data
  loadStation() {
    this.lineStationService.search(
      this.pagination.currentPage,
      this.pagination.itemsPerPage,
      this.paramSearch)
      .subscribe(res => {
        // debugger;
        this.pagination = res.pagination;
        this.linestations = res.result;
        // console.log("Station data : ", this.linestations);
      });
    setTimeout(() => {
    }, 3000);
  }

  // 刪除站台function
  deleteStation(item: LineStation) {
    this.alertify.confirm('Delete ' + item.line_ID + ' Line ' + item.station_Name + ' Station', 'Are you sure you want to delete this station??', () => {
      this.lineStationService.deleteStation(item).subscribe(() => {
        this.loadStation();
        this.alertify.success('station has been deleted!');
      }, error => {
        this.alertify.error('This station is using!');
      });
    });
  }

  // 換頁
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadStation();
  }

  // open add page
  addStation() {
    this.linestation = {};
    this.lineStationService.changeLineStation(this.linestation);
    this.lineStationService.changeFlag("0");
    // console.log(this.lineStationService.flagSource.value);
    this.router.navigate(["/station/add"]);
  }

  // open edit page
  changeToEdit(item: LineStation) {
    // console.log(item);
    this.lineStationService.changeLineStation(item);
    this.lineStationService.changeFlag("1");
    // console.log(this.defectreasonService.flagSource.value);
    this.router.navigate(["/station/edit"]);
  }


  // clear inputbox
  clearSearch() {
    // bind API helper
    this.paramSearch = "";
  }

  backList() {
    this.router.navigate(["/officekanban/officekanban"]);
  }

}
