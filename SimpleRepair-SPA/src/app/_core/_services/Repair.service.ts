import { Repair } from './../_models/Repair';
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { PaginatedResult } from "../_models/pagination";
//import { ITOfficeKanban } from "../_models/ITOfficeKanban";

@Injectable({
  providedIn: "root",
})
export class RepairService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // 新增維修資料
  createITOfficeKanban(ee: Repair, stationName: string) {
    // console.log("Line_ID: ", ee);
    // *** API接引數要用Class接 ***
    return this.http.post(this.baseUrl + "Repair/createkanban/", {
      Line_ID: ee.unitCode,
      Line_Name: ee.unitName,
      Station_Name: stationName,
    });
  }

  // getDataOrgChart = () => this.http.get<any>(this.baseUrl + "Repair/factoryorgchart");

}
