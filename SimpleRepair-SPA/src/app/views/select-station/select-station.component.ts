import { Repair } from "./../../_core/_models/Repair";
import { Component, OnInit, EventEmitter } from "@angular/core";
import { LineStation } from "../../_core/_models/LineStation";
import { RepairService } from "../../_core/_services/Repair.service";
import { StationService } from "../../_core/_services/Station.service";

import { AlertifyService } from "../../_core/_services/alertify.service";
import { BsModalRef } from "ngx-bootstrap/modal";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../environments/environment";

@Component({
  selector: "app-select-station",
  templateUrl: "./select-station.component.html",
  styleUrls: ["./select-station.component.scss"],
})
export class SelectStationComponent implements OnInit {
  baseUrl = environment.apiUrl;
  linestations: LineStation[]; // model container

  // outsidenum: number[];

  repair: any = {}; // 父component的值

  eventAddtoCart: EventEmitter<any> = new EventEmitter();

  constructor(
    private repairService: RepairService,
    private stationService: StationService,
    private alertify: AlertifyService,
    private http: HttpClient,
    public bsModalRef: BsModalRef
  ) {}

  ngOnInit(): void {
    this.getDataOrgChart(this.repair);
  }

  getDataOrgChart(repairs: Repair) {
    return this.http
      .post<any>(this.baseUrl + "LineStation/getstationname/", repairs)
      .subscribe((res) => {
        this.linestations = res;
        // console.log(this.linestations);
        // count rowcolumn
      //   this.outsidenum = this.linestations
      //   .map((item) => item.station_ID)
      //   .filter((value, index, self) => self.indexOf(value) === index)
      //   .sort();
      // console.log(this.outsidenum); // 計算幾列
      });
  }

  // getByLineNum_2ndWay(lineNum: number) {
  //   return this.linestations.filter((item) => item.station_ID === lineNum);
  // }

  // 看板建立站台
  StationRepairClick(father: Repair, btn: LineStation) {
    // console.log('線別(父component) : ', father);
    // console.log('按鈕的值(抓station name) : ', btn);
    this.alertify.confirm(
      "Confirm",
      "Repair " + btn.station_Name + " this station?",
      () => {
        this.repairService
          .createITOfficeKanban(father, btn.station_Name)
          .subscribe(
            () => {
              this.UpdateStation(btn);
            },
            (error) => {
              this.alertify.error(error);
            }
          );
      }
    );
  }

  // 更新站台狀態
  UpdateStation(lineStation: LineStation) {
    lineStation.isUsing = "Y";
    this.stationService.updateStation(lineStation).subscribe(
      () => {
        this.alertify.success("Add succeed");
        this.eventAddtoCart.emit();
        this.bsModalRef.hide();
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }
}
