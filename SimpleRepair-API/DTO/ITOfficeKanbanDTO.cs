using System;

namespace SimpleRepair_API.DTO
{
  public class ITOfficeKanbanDTO
  {
    public string code { get; set; }
    public string FactoryID { get; set; }
    public string Line_ID { get; set; }
    public string Line_Name { get; set; }
    public string Station_Name { get; set; }
    public DateTime SignRepairTime { get; set; }
    public DateTime? StartRepairTime { get; set; }
    public string IsRepaired { get; set; }
    public DateTime? IsRepairedTime { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string Solution { get; set; }
  }

  public class SearchParam
  {
    public string isrepaired { get; set; }
  }
}