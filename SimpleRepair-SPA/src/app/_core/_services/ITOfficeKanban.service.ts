import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { PaginatedResult } from "../_models/pagination";
import { ITOfficeKanban } from "../_models/ITOfficeKanban";

@Injectable({
  providedIn: "root",
})
export class ITOfficeKanbanService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // 搜尋
  search(
    page?,
    itemsPerPage?,
    paramSearch?: String
  ): Observable<PaginatedResult<ITOfficeKanban[]>> {
    const paginatedResult: PaginatedResult<
      ITOfficeKanban[]
    > = new PaginatedResult<ITOfficeKanban[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }
    let url = this.baseUrl + "OfficeKanban/search";
    // console.log(paramSearch);
    return this.http
      .post<any>(url, { isrepaired: paramSearch }, { observe: "response", params })
      .pipe(
        map((response) => {
          // console.log(response);
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

  // 接單(改狀態)
  getOrder(iTOfficeKanban: ITOfficeKanban) {
    // console.log(iTOfficeKanban);
    return this.http.post(this.baseUrl + "OfficeKanban/getorder/", {
      code: iTOfficeKanban.code,
      FactoryID: iTOfficeKanban.factoryID,
      Line_ID: iTOfficeKanban.line_ID,
      Line_Name: iTOfficeKanban.line_Name,
      Station_Name: iTOfficeKanban.station_Name,
      SignRepairTime: iTOfficeKanban.signRepairTime,
      IsRepaired: iTOfficeKanban.isRepaired,
      IsRepairedTime: iTOfficeKanban.isRepairedTime,
      Status: iTOfficeKanban.status,
      Description: iTOfficeKanban.description,
      Solution: iTOfficeKanban.solution,
    });
  }

  // 完成按鈕(改狀態 + 填寫Description & Solution)
  finishOrder(iTOfficeKanban: ITOfficeKanban) {
    return this.http.post(this.baseUrl + "OfficeKanban/finishorder/", {
      code: iTOfficeKanban.code,
      FactoryID: iTOfficeKanban.factoryID,
      Line_ID: iTOfficeKanban.line_ID,
      Line_Name: iTOfficeKanban.line_Name,
      Station_Name: iTOfficeKanban.station_Name,
      SignRepairTime: iTOfficeKanban.signRepairTime,
      StartRepairTime: iTOfficeKanban.startRepairTime,
      IsRepaired: iTOfficeKanban.isRepaired,
      IsRepairedTime: iTOfficeKanban.isRepairedTime,
      Status: iTOfficeKanban.status,
      Description: iTOfficeKanban.description,
      Solution: iTOfficeKanban.solution,
    });
  }
}
