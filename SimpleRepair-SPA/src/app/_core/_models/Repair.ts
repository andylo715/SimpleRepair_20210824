export interface Repair {
  Id: number;
  Level: number;
  ParentID: number;
  unitCode: string;
  unitName: string;
  SortSeq: number;
  lineNum: number;
  RowCount: number;
  status: string;
}
