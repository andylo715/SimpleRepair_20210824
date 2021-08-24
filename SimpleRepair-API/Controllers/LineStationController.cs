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
using SimpleRepair_API.Models;

namespace SimpleRepair_API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class LineStationController : ControllerBase
  {
    private readonly ILineStationService _serviceLineStation;
    private readonly DataContext ctx;
    private string factory;
    public LineStationController(ILineStationService serviceLineStation, DataContext context, IConfiguration configuration)
    {
      ctx = context;
      _serviceLineStation = serviceLineStation;
      factory = configuration.GetSection("AppSettings:Factory").Value;
    }

    [HttpPost("search")]
    public async Task<IActionResult> DataSearch([FromQuery] PaginationParams param, StationSearchParam paramSearch)
    {
      var lists = await _serviceLineStation.SearchLineStation(param, paramSearch);
      Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
      return Ok(lists);
    }

    [HttpGet("getline")]
    public async Task<IActionResult> GetLine() => Ok(await _serviceLineStation.GetAllLine());

    [HttpPost("getstationname")]
    public ActionResult GetStationName(VFactoryIndexOC lineInfo)
    {
      return Ok(_serviceLineStation.getLineStationName(lineInfo.UnitCode));
    }

    [HttpPost("stationfinish")]
    public async Task<IActionResult> UpdateStationForFinish(ITOfficeKanbanDTO iTOfficeKanbanDTO)
    {
      var test = ctx.LineStation.Where(x => x.Line_ID == iTOfficeKanbanDTO.Line_ID && x.Station_Name == iTOfficeKanbanDTO.Station_Name).Select(p => p).FirstOrDefault();
      // var test = _serviceLineStation.getLineOneStation(iTOfficeKanbanDTO.Line_ID, iTOfficeKanbanDTO.Station_Name);
      // 改變站台狀態(使該站台能再次回報)
      LineStationDTO dTO = new LineStationDTO()
      {
        Line_ID = test.Line_ID,
        Station_ID = test.Station_ID,
        Station_Name = test.Station_Name,
        Station_PIC = test.Station_PIC,
        Station_PhoneNum = test.Station_PhoneNum,
        Station_IT_PIC = test.Station_IT_PIC,
        Station_IT_PhoneNum = test.Station_IT_PhoneNum,
        IsUsing = "N",
        UpdateTime = DateTime.Now
      };
      ctx.Entry<LineStation>(test).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
      try
      {
        if (await _serviceLineStation.Update(dTO))
        {
          return Ok();
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      return BadRequest($"Updating {iTOfficeKanbanDTO.Station_Name} status failed on save");
    }

    // 新增站台
    [HttpPost("addStation")]
    public async Task<IActionResult> AddStation(LineStationDTO lineStationDTO)
    {
      var stationid = (from T1 in ctx.LineStation
                       orderby T1.Station_ID descending
                       select T1.Station_ID).FirstOrDefault();
      lineStationDTO.Station_ID = stationid + 1;
      if (await _serviceLineStation.CheckStationExists(lineStationDTO.Line_ID, lineStationDTO.Station_ID))
      {
        return BadRequest("LineStation already exists!");
      }
      lineStationDTO.UpdateTime = DateTime.Now;
      if (await _serviceLineStation.Add(lineStationDTO))
      {
        return Ok();
      }
      throw new Exception("Creating the LineStation failed on save");
    }

    // 修改站台
    [HttpPost("updateStation")]
    public async Task<IActionResult> UpdateStation(LineStationDTO lineStationDTO)
    {
      lineStationDTO.UpdateTime = DateTime.Now;
      if (await _serviceLineStation.Update(lineStationDTO))
      {
        return Ok();
      }
      return BadRequest($"Updating Defect Reason {lineStationDTO.Station_Name} failed on save");
    }

    // 刪除站台
    [HttpPost("deleteStation")]
    public async Task<IActionResult> DeleteModelOperation(LineStationDTO lineStationDTO)
    {
      if (await _serviceLineStation.Delete(lineStationDTO))
      {
        return Ok();
      }
      return BadRequest($"Station Delete Fail");
    }

  }
}