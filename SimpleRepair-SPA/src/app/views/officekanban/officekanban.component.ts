import { TestBed } from "@angular/core/testing";
import { Component, OnInit } from "@angular/core";
import { PaginatedResult, Pagination } from "../../_core/_models/pagination";
import { ITOfficeKanban } from "../../_core/_models/ITOfficeKanban";
import { ITOfficeKanbanService } from "../../_core/_services/ITOfficeKanban.service";
import { Router } from "@angular/router";
import { AlertifyService } from "../../_core/_services/alertify.service";
import { NgxSpinnerService } from "ngx-spinner";

import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { ShowDetailComponent } from "../show-detail/show-detail.component";
import { FinishOrderComponent } from "../finish-order//finish-order.component";
import { formatDate } from "@angular/common";

@Component({
  selector: "app-officekanban",
  templateUrl: "./officekanban.component.html",
  styleUrls: ["./officekanban.component.scss"],
})
export class OfficekanbanComponent implements OnInit {
  itofficekanbans: ITOfficeKanban[]; // model container
  paramSearch: string;
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  lastKanbanCount: number = 0;

  bsModalRef: BsModalRef;

  constructor(
    private itofficekanbanService: ITOfficeKanbanService,
    private alertify: AlertifyService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) {}

  ngOnInit() {
    this.spinner.show();
    this.paramSearch = "N";
    this.loadITOfficeKanban();
    this.spinner.hide();
    setInterval(() => {
      this.chkDataCount();
    }, 60000);
  }

  // get data
  loadITOfficeKanban() {
    this.itofficekanbanService
      .search(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.paramSearch
      )
      .subscribe((res) => {
        // debugger;
        this.pagination = res.pagination;
        this.itofficekanbans = res.result;
        // console.log("Kanban data : ", this.itofficekanbans);
      });

    setTimeout(() => {}, 3000);
  }

  // 比對資料
  chkDataCount() {
    this.itofficekanbanService
      .search(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.paramSearch
      )
      .subscribe((res) => {
        this.lastKanbanCount = res.result.length;
        // 資料筆數有變多就播放音效
        if (this.lastKanbanCount > this.itofficekanbans.length) {
          this.sound();
        }
        this.itofficekanbans = res.result;
      });
    setTimeout(() => {}, 3000);
  }

  sound() {
    // C:\inetpub\wwwroot\SimpleRepair_T\SimpleRepair-SPA\assets\audio
    let audio = new Audio();
    console.log("播放音效!");
    audio.src = "../../../assets/audio/airhorn.mp3";
    audio.load();
    audio.play();
  }

  // 判斷欄位顏色
  rowColor(chkdate: Date, status: string) {
    var today = new Date();
    var orgtime = Number(formatDate(chkdate, "dd", "en-US", "+0800"));
    var result = today.getDate() - orgtime;
    // console.log("result", result);

    // close repair
    if (status == "Y") {
      return "white";
    } else {
      // over 2 days
      if (result > 2) {
        return "red";
      }
      // during 1 to 2 days
      else if (result == 1 || result == 2) {
        return "yellow";
      }
      // with 1 day
      else {
        return "white";
      }
    }
  }

  // 換頁
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadITOfficeKanban();
  }

  // 接單按鈕
  getOrder(orderlist: ITOfficeKanban) {
    console.log(orderlist);
    this.alertify.confirm(
      "Confirm",
      "list: " +
        orderlist.code +
        "<br>" +
        "Line: " +
        orderlist.line_Name +
        "<br>" +
        "Get this order?",
      () => {
        this.itofficekanbanService.getOrder(orderlist).subscribe(
          () => {
            this.loadITOfficeKanban();
            this.alertify.success("edit succeed");
          },
          (error) => {
            this.alertify.error(error);
          }
        );
      }
    );
  }

  // 完成按鈕
  finishOrder(itofficekanban: ITOfficeKanban) {
    const initialState = { itofficekanban };
    this.bsModalRef = this.modalService.show(FinishOrderComponent, {
      class: "modal-xl",
      initialState,
    });
    // 偵測子component event
    this.bsModalRef.content.eventAddtoCart.subscribe((res) => {
      this.loadITOfficeKanban();
    });
  }

  // detail按鈕
  showDetail(itofficekanban: ITOfficeKanban) {
    const initialState = { itofficekanban };
    this.bsModalRef = this.modalService.show(ShowDetailComponent, {
      class: "modal-info cus-modal-xxl",
      initialState,
    });
  }

  // open station list page
  maintainStation() {
    this.router.navigate(["/station/station"]);
  }
}
