using System;

namespace SimpleRepair_API.DTO
{
  public class LineStationDTO
  {
    public string Line_ID { get; set; }
    public int Station_ID { get; set; }
    public string Station_Name { get; set; }
    public string Station_PIC { get; set; }
    public string Station_PhoneNum { get; set; }
    public string Station_IT_PIC { get; set; }
    public string Station_IT_PhoneNum { get; set; }
    public string IsUsing { get; set; }
    public DateTime? UpdateTime { get; set; }
  }

  public class StationSearchParam
  {
    public string stationName { get; set; }
  }
}