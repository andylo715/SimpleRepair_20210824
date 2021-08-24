using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SimpleRepair_API._Services.Interfaces;
using SimpleRepair_API.Helpers;
using SimpleRepair_API.DTO;
using SimpleRepair_API.Data;

namespace SimpleRepair_API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class RepairController : ControllerBase
  {
    private readonly ITOrgService _serviceTOrg;
    private readonly DataContext ctx;
    private string factory;
    private string factoryid;
    public RepairController(ITOrgService serviceTOrg, DataContext context, IConfiguration configuration)
    {
      ctx = context;
      _serviceTOrg = serviceTOrg;
      // 廠區 使用factoryid
      factory = configuration.GetSection("AppSettings:Factory").Value;
      factoryid = (factory == "SHC" || factory == "SHCt") ? "C" : (factory == "SPC" || factory == "SPCt") ? "D" : (factory == "CB" || factory == "CBt") ? "E" : (factory == "TSH" || factory == "TSHt") ? "U" : "NaN";
    }

    [HttpGet("factoryorgchart")]
    public ActionResult FactoryOrgChart() => Ok(_serviceTOrg.getFactoryIndexOC());

    [HttpPost("createkanban")]
    public async Task<IActionResult> CreateITOfficeKanban(ITOfficeKanbanDTO iTOfficeKanbanDTO)
    {
      // 檢查是否已經報修
      var chkIsRepair = ctx.ITOfficeKanban.Where(x => x.FactoryID == factoryid && x.SignRepairTime.Date == DateTime.Today && x.Line_ID == iTOfficeKanbanDTO.Line_ID && x.Station_Name == iTOfficeKanbanDTO.Station_Name && x.StartRepairTime == null).ToList();
      if (chkIsRepair.Count > 0)
      {
        // 重複報修
      }
      else
      {
        // 流水號
        var serialdata = (from a in ctx.ITOfficeKanban
                          where a.FactoryID == factoryid && a.SignRepairTime.Date == DateTime.Today
                          orderby a.code descending
                          select a.code).FirstOrDefault();
        string serialnum = serialdata == null ? "001" : (Convert.ToInt32(serialdata.Substring(11)) + 1).ToString().PadLeft(3, '0');
        // code=廠區(1) + "SR" + 年(4) + 月(2) + 日(2) + 流水號(3)
        iTOfficeKanbanDTO.code = factoryid + "SR" + DateTime.Now.ToString("yyyyMMdd") + serialnum;
        iTOfficeKanbanDTO.FactoryID = factoryid;
        iTOfficeKanbanDTO.SignRepairTime = DateTime.Now;
        iTOfficeKanbanDTO.IsRepaired = "N";
        iTOfficeKanbanDTO.Status = "N";
        iTOfficeKanbanDTO.IsRepairedTime = null;
        iTOfficeKanbanDTO.Solution = "";
        iTOfficeKanbanDTO.Description = "";

        if (await _serviceTOrg.Add(iTOfficeKanbanDTO))
        {
          
          return Ok();
          // return CreatedAtRoute("GetDefectReasons", new { });
        }
      }
      throw new Exception("Creating the Office Kanban failed on save");
    }
  }
}