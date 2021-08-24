export interface ITOfficeKanban {
  code: string;
  factoryID: string;
  line_ID: string;
  line_Name: string;
  station_Name: string;
  signRepairTime: Date;
  startRepairTime: Date
  isRepaired: string;
  isRepairedTime: Date;
  status: string;
  description: string;
  solution: string;
}