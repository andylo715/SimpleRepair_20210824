using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SimpleRepair_API._Repositories.Interfaces;
using SimpleRepair_API._Services.Interfaces;
using SimpleRepair_API.DTO;
using SimpleRepair_API.Helpers;
using SimpleRepair_API.Models;
using SimpleRepair_API.Data;

namespace SimpleRepair_API._Services.Services
{
  public class LineStationService : ILineStationService
  {
    private readonly ILineStationRepository _ilinestationRepo;
    private readonly ITOrgRepository _itorgRepo;
    private readonly DataContext ctx;
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configMapper;
    private string factory;

    public LineStationService(ILineStationRepository ilinestationRepo, ITOrgRepository itorgRepo, DataContext context, IMapper mapper, MapperConfiguration configMapper, IConfiguration configuration)
    {
      ctx = context;
      _ilinestationRepo = ilinestationRepo;
      _itorgRepo = itorgRepo;
      _mapper = mapper;
      _configMapper = configMapper;
      factory = configuration.GetSection("AppSettings:Factory").Value;
    }

    public async Task<PagedList<LineStationDTO>> SearchLineStation(PaginationParams paginationParams, StationSearchParam searchParam)
    {
      var query = _ilinestationRepo.FindAll();
      if (!String.IsNullOrEmpty(searchParam.stationName))
      {
        query = query.Where(x => x.Station_Name.Contains(searchParam.stationName));
      }
      var list = query.ProjectTo<LineStationDTO>(_configMapper).OrderBy(x => x.Line_ID);
      return await PagedList<LineStationDTO>.CreateAsync(list, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<object> GetAllLine()
    {
      return await ctx.TOrg.Select(x => new { x.Line_ID, x.Line_Name }).Distinct().OrderBy(x => x.Line_ID).ToListAsync();
    }

    public List<LineStation> getLineStationName(string lineID)
    {
      List<LineStation> result = new List<LineStation>();
      LineStation res = new LineStation();
      var test = ctx.LineStation.Where(x => x.Line_ID == lineID).Select(p => p).OrderBy(a => a.Station_ID).AsQueryable();
      foreach (var t in test)
      {
        res = new LineStation()
        {
          Line_ID = t.Line_ID,
          Station_ID = t.Station_ID,
          Station_Name = t.Station_Name,
          Station_PIC = t.Station_PIC,
          Station_PhoneNum = t.Station_PhoneNum,
          Station_IT_PIC = t.Station_IT_PIC,
          Station_IT_PhoneNum = t.Station_IT_PhoneNum,
          IsUsing = t.IsUsing
        };
        result.Add(res);
        res = null;
      }
      return result;
    }

    // 單一站台(完成維修後呼叫)
    public LineStation getLineOneStation(string lineID, string stationname)
    {
      var stationinfo = ctx.LineStation.Where(x => x.Line_ID == lineID && x.Station_Name == stationname).Select(p => p).FirstOrDefault();
      return stationinfo;
    }
    public async Task<bool> Add(LineStationDTO model)
    {
      var lineStation = _mapper.Map<LineStation>(model);
      _ilinestationRepo.Add(lineStation);
      return await _ilinestationRepo.SaveAll();
    }

    public async Task<bool> Update(LineStationDTO model)
    {
      var lineStation = _mapper.Map<LineStation>(model);
      _ilinestationRepo.Update(lineStation);
      return await _ilinestationRepo.SaveAll();
    }

    public async Task<bool> Delete(LineStationDTO model)
    {
      var lineStation = _mapper.Map<LineStation>(model);
      _ilinestationRepo.Remove(lineStation);
      return await _ilinestationRepo.SaveAll();
    }

    public async Task<bool> CheckStationExists(string lineID, int stationID)
    {
      return await _ilinestationRepo.CheckStationExists(lineID, stationID);
    }
  }
}