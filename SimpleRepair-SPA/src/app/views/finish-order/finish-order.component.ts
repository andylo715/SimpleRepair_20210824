import { Component, OnInit, EventEmitter } from '@angular/core';
import { AlertifyService } from "../../_core/_services/alertify.service";

import { LineStation } from "../../_core/_models/LineStation";
import { ITOfficeKanbanService } from "../../_core/_services/ITOfficeKanban.service";
import { StationService } from "../../_core/_services/Station.service";
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-finish-order',
  templateUrl: './finish-order.component.html',
  styleUrls: ['./finish-order.component.scss']
})
export class FinishOrderComponent implements OnInit {
  itofficekanban: any = {};
  // test: LineStation;

  eventAddtoCart: EventEmitter<any> = new EventEmitter();

  constructor(
    private itofficekanbanService: ITOfficeKanbanService,
    private stationService: StationService,
    private alertify: AlertifyService,
    public bsModalRef: BsModalRef,
  ) { }

  ngOnInit(): void {
    console.log('data', this.itofficekanban);
  }

  // 儲存按鈕(改狀態 + 填寫Description & Solution)
  finish() {
    this.itofficekanbanService.finishOrder(this.itofficekanban).subscribe(
      () => {
        this.UpdateStation();
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }
  //lineStation: LineStation
  // 更新站台狀態
  UpdateStation() {
    this.stationService.updateStationForFinish(this.itofficekanban).subscribe(
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
