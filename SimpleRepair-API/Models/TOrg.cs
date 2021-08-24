using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SimpleRepair_API.Models
{
  public class TOrg
  {
    [Key]
    [Column(Order = 0)]
    [StringLength(1)]
    public string Factory_ID { get; set; }
    [Key]
    [Column(Order = 1)]
    [StringLength(1)]
    public string PDC_ID { get; set; }
    [Column(Order = 2)]
    [StringLength(1)]
    public string PDC_Name { get; set; }
    [Key]
    [Column(Order = 3)]
    [StringLength(3)]
    public string Line_ID { get; set; }
    [Column(Order = 4)]
    [StringLength(3)]
    public string Line_Name { get; set; }
    [Key]
    [Column(Order = 5)]
    [StringLength(3)]
    public string Dept_ID { get; set; }
    [StringLength(10)]
    public string Building { get; set; }
    public int? Line_Seq { get; set; }
    public int? Status { get; set; }
    [StringLength(1)]
    public string Dept_Kind { get; set; }
    public DateTime? Update_Time { get; set; }
    [StringLength(16)]
    public string Updated_By { get; set; }
  }

  public class VFactoryIndexOC
  {
    public int Id { get; set; }
    public int Level { get; set; }
    public int? ParentID { get; set; }
    public string UnitCode { get; set; }   // Factory,ProdDept,Group,Cell
    public string UnitName { get; set; }
    public int? SortSeq { get; set; }      // Sort sequence by Parent Id
    public string LineNum { get; set; }      // The first LineNum of Child level
    public int? RowCount { get; set; }
    public string status { get; set; } // check status
  }
}