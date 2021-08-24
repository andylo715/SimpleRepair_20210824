import { LineName } from "./../_models/LineName";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { PaginatedResult } from "../_models/pagination";

import { LineStation } from "../_models/LineStation";
import { ITOfficeKanban } from "../_models/ITOfficeKanban";
import { error } from "protractor";

@Injectable({
  providedIn: "root",
})
export class StationService {
  baseUrl = environment.apiUrl;
  linenames: LineName[]; // line dropdownlist container

  stationSource = new BehaviorSubject<Object>({});
  currentstation = this.stationSource.asObservable();
  flagSource = new BehaviorSubject<string>("0");
  currentFlag = this.flagSource.asObservable();
  constructor(private http: HttpClient) { }

  // 搜尋
  search(
    page?,
    itemsPerPage?,
    paramSearch?: String
  ): Observable<PaginatedResult<LineStation[]>> {
    const paginatedResult: PaginatedResult<LineStation[]> = new PaginatedResult<
      LineStation[]
    >();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }
    let url = this.baseUrl + "LineStation/search";
    // console.log(paramSearch);
    return this.http
      .post<any>(
        url,
        { stationName: paramSearch },
        { observe: "response", params }
      )
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }

  // 線別下拉選單
  getLine() {
    return this.http.get<any>(this.baseUrl + "LineStation/getline");
  }

  // 新增站台
  addStation(linestaion: LineStation) {
    return this.http.post(this.baseUrl + "LineStation/addStation/", linestaion);
  }

  // 修改站台
  updateStation(linestaion: LineStation) {
    return this.http.post(
      this.baseUrl + "LineStation/updateStation/",
      linestaion
    );
  }

  // 修改站台(完成維修呼叫)
  updateStationForFinish(iTOfficeKanban: ITOfficeKanban) {
    return this.http.post(
      this.baseUrl + "LineStation/stationfinish/",
      iTOfficeKanban
      // {
      // code: iTOfficeKanban.code,
      // FactoryID: iTOfficeKanban.factoryID,
      // Line_ID: iTOfficeKanban.line_ID,
      // Line_Name: iTOfficeKanban.line_Name,
      // Station_Name: iTOfficeKanban.station_Name,
      // SignRepairTime: iTOfficeKanban.signRepairTime,
      // StartRepairTime: iTOfficeKanban.startRepairTime,
      // IsRepaired: iTOfficeKanban.isRepaired,
      // IsRepairedTime: iTOfficeKanban.isRepairedTime,
      // Status: iTOfficeKanban.status,
      // Description: iTOfficeKanban.description,
      // Solution: iTOfficeKanban.solution,
      // }
    );
  }

  // 刪除站台
  deleteStation(linestaion: LineStation) {
    return this.http.post(
      this.baseUrl + "LineStation/deleteStation/",
      linestaion
    );
  }

  changeLineStation(linestaion: LineStation) {
    this.stationSource.next(linestaion);
  }

  changeFlag(flag: string) {
    this.flagSource.next(flag);
  }
}
