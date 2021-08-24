using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SimpleRepair_API.Models
{
  public class LineStation
  {
    [Key]
    [Column(Order = 0)]
    public string Line_ID { get; set; }
    [Key]
    [Column(Order = 1)]
    public int Station_ID { get; set; }
    public string Station_Name { get; set; }
    public string Station_PIC { get; set; }
    public string Station_PhoneNum { get; set; }
    public string Station_IT_PIC { get; set; }
    public string Station_IT_PhoneNum { get; set; }
    public string IsUsing { get; set; }
    public DateTime? UpdateTime { get; set; }
  }
}