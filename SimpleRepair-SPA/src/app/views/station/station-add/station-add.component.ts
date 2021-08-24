import { Component, OnInit } from '@angular/core';
import { StationService } from "../../../_core/_services/Station.service";
import { AlertifyService } from "../../../_core/_services/alertify.service";
import { Router } from "@angular/router";
import { Select2OptionData } from "ng-select2";

@Component({
  selector: 'app-station-add',
  templateUrl: './station-add.component.html',
  styleUrls: ['./station-add.component.scss']
})
export class StationAddComponent implements OnInit {
  linestation: any = {};
  lineNameList: Array<Select2OptionData>;
  flag = "100";

  constructor(
    private stationService: StationService,
    private alertify: AlertifyService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.stationService.currentstation.subscribe((linestation) => (this.linestation = linestation));
    // flag 1=add, 0=edit
    this.stationService.currentFlag.subscribe((flag) => (this.flag = flag));
    if (this.flag === '0') this.linestation.line_ID = "";
    this.getAllLineName();

  }

  // line 下拉選單
  getAllLineName() {
    this.stationService.getLine().subscribe(
      (data) => {
        // console.log(data);
        this.lineNameList = data.map((item) => {
          // console.log(item);
          return { id: item.line_ID, text: item.line_Name };
        });
      },
      (error) => {
        this.alertify.error(error);
      }
    )
  }

  backList() {
    this.router.navigate(["/station/station"]);
  }

  saveAndNext() {
    // console.log("save and next");
    if (this.flag === "0") {
      this.linestation.isUsing = "N";
      this.stationService.addStation(this.linestation).subscribe(
        () => {
          this.alertify.success("Add succeed");
          this.linestation = {};
          // save page
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    }
  }

  save() {
    // console.log(this.linestation);
    if (this.flag === "0") {
      // console.log("save add");
      this.linestation.isUsing = "N";
      this.stationService.addStation(this.linestation).subscribe(
        () => {
          this.alertify.success("Add succeed");
          this.router.navigate(["/station/station"]);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    } else {
      // console.log("save edit");
      this.stationService.updateStation(this.linestation).subscribe(
        () => {
          this.alertify.success("Updated succeed");
          this.router.navigate(["/station/station"]);
        },
        (error) => {
          this.alertify.error(error);
        }
      );

    }
  }

  clear() {
    this.linestation = {};
  }

}
