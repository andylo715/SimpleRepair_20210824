using System;

namespace SimpleRepair_API.DTO
{
  public class TOrgDTO
  {
    public string Factory_ID { get; set; }
    public string PDC_ID { get; set; }
    public string PDC_Name { get; set; }
    public string Line_ID { get; set; }
    public string Line_Name { get; set; }
    public string Dept_ID { get; set; }
    public string Building { get; set; }
    public int? Line_Seq { get; set; }
    public int? Status { get; set; }
    public string Dept_Kind { get; set; }
    public DateTime? Update_Time { get; set; }
    public string Updated_By { get; set; }
  }
}