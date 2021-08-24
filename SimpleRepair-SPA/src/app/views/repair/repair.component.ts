import { TestBed } from "@angular/core/testing";
import { Component, OnInit } from "@angular/core";
import { RepairService } from "../../_core/_services/Repair.service";
import { Repair } from "../../_core/_models/Repair";
import { SelectStationComponent } from '../select-station/select-station.component'

import { AlertifyService } from "../../_core/_services/alertify.service";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: "app-repair",
  templateUrl: "./repair.component.html",
  styleUrls: ["./repair.component.scss"],
})
export class RepairComponent implements OnInit {
  baseUrl = environment.apiUrl;
  repairs: Repair[]; // model container
  noData: boolean = false;
  outsidenum: number[];

  bsModalRef: BsModalRef;

  constructor(
    private repairService: RepairService,
    private http: HttpClient,
    private alertify: AlertifyService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    this.spinner.show();
    this.getDataOrgChart();
    this.spinner.hide();
    setInterval(() => { this.getDataOrgChart() }, 60000);
  }

  // 呼叫API 組織圖
  getDataOrgChart() {
    return this.http
      .get<any>(this.baseUrl + "Repair/factoryorgchart")
      .subscribe(
        (data) => {
          this.repairs = data;
          // console.log("before", this.repairs);
          // count rowcolumn
          this.outsidenum = this.repairs
            .map((item) => item.lineNum)
            .filter((value, index, self) => self.indexOf(value) === index)
            .sort();
          // console.log(this.outsidenum); // 計算幾列
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }

  getByLineNum_2ndWay(lineNum: number) {
    return this.repairs.filter((item) => item.lineNum === lineNum);
  }

  // 報修按鈕
  // 2021/06/01 要改成多加站台
  // repairClick(repair: Repair) {
  //   // console.log(repair);
  //   this.alertify.confirm("Confirm", "Repair " + repair.unitName + "?",
  //     () => {
  //       this.repairService.createITOfficeKanban(repair).subscribe(
  //         () => {
  //           this.alertify.success("Add succeed");
  //           this.getDataOrgChart();
  //         },
  //         (error) => {
  //           this.alertify.error(error);
  //           this.getDataOrgChart();
  //         }
  //       );
  //     });
  // }

  // 站台報修按鈕
  // 呼叫站台子component
  repairClick(repair: Repair) {
    const initialState = { repair };
    this.bsModalRef = this.modalService.show(SelectStationComponent, { class: 'modal-xl', initialState });
    // 偵測子component event
    this.bsModalRef.content.eventAddtoCart.subscribe(res => {
      this.getDataOrgChart();
    });
  }

}
