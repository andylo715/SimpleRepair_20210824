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
  public class OfficeKanbanController : ControllerBase
  {
    private readonly IOfficeKanbanService _serviceOfficeKanban;
    // private readonly ILineStationService _serviceLineStation;
    private string factory;
    public OfficeKanbanController(IOfficeKanbanService serviceOfficeKanban, ILineStationService serviceLineStation, IConfiguration configuration)
    {
      _serviceOfficeKanban = serviceOfficeKanban;
      // _serviceLineStation = serviceLineStation;
      factory = configuration.GetSection("AppSettings:Factory").Value;
    }

    [HttpPost("search")]
    public async Task<IActionResult> DataSearch([FromQuery] PaginationParams param, SearchParam isrepaired)
    {
      var lists = await _serviceOfficeKanban.SearchOfficeKanban(param, isrepaired);
      Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
      return Ok(lists);
    }

    // 狀態邏輯: Open = N, Repair = R, Close = Y

    [HttpPost("getorder")]
    public async Task<IActionResult> GetOrder(ITOfficeKanbanDTO iTOfficeKanbanDTO)
    {
      iTOfficeKanbanDTO.Status = "R";
      iTOfficeKanbanDTO.StartRepairTime = DateTime.Now;
      if (await _serviceOfficeKanban.Update(iTOfficeKanbanDTO))
      {
        return NoContent();
      }
      return BadRequest($"Updating {iTOfficeKanbanDTO.code} status failed on save");
    }

    [HttpPost("finishorder")]
    public async Task<IActionResult> FinishOrder(ITOfficeKanbanDTO iTOfficeKanbanDTO)
    {
      iTOfficeKanbanDTO.Status = "Y";
      iTOfficeKanbanDTO.IsRepaired = "Y";
      iTOfficeKanbanDTO.IsRepairedTime = DateTime.Now;
      if (await _serviceOfficeKanban.Update(iTOfficeKanbanDTO))
      {
        return Ok();
      }
      return BadRequest($"Updating {iTOfficeKanbanDTO.code} status failed on save");
    }

  }
}