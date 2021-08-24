using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;
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
  public class TOrgService : ITOrgService
  {
    private readonly ITOrgRepository _itorgRepo;
    private readonly DataContext ctx;
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configMapper;
    private string factory;

    public TOrgService(ITOrgRepository itorgRepo, DataContext context, IMapper mapper, MapperConfiguration configMapper, IConfiguration configuration)
    {
      ctx = context;
      _itorgRepo = itorgRepo;
      _mapper = mapper;
      _configMapper = configMapper;
      factory = configuration.GetSection("AppSettings:Factory").Value;
    }

    public List<VFactoryIndexOC> getFactoryIndexOC()
    {

      //the logic for Factory OC if had 4 level   Factory > Build > Group > Cell

      #region 組織圖

      int ListID = 0;
      int SortSequence = 0;

      List<VFactoryIndexOC> result = new List<VFactoryIndexOC>();
      //var factoryorgdata = _itorgRepo.FindAll();
      VFactoryIndexOC VFIOC = new VFactoryIndexOC();
      ListID = 1;
      SortSequence = 1;

      #region Get Level1

      var FactoryID = (from a in ctx.TOrg
                       select a.Factory_ID).FirstOrDefault();
      VFactoryIndexOC res = new VFactoryIndexOC()
      {
        Id = ListID,
        Level = 1,
        ParentID = null,
        UnitCode = FactoryID,
        UnitName = (FactoryID == "C") ? "SHC" : (FactoryID == "D") ? "SPC" : (FactoryID == "E") ? "CB" : (FactoryID == "U") ? "TSH" : "NaN",
        SortSeq = SortSequence,
        LineNum = (1).ToString().PadLeft(2, '0'),
        RowCount = null,
      };
      result.Add(res);

      #endregion

      res = null;
      SortSequence = 0;

      #region Get Level2

      var PDCID2 = ctx.TOrg.Select(p => new { p.Factory_ID, p.PDC_ID, p.PDC_Name }).Distinct().OrderBy(a => a.PDC_ID).AsQueryable();
      switch (FactoryID)
      {
        case "C":
          break;
        case "D":
          break;
        case "E":
          break;
        case "F":
          PDCID2 = PDCID2.Where(a => string.Compare(a.PDC_ID, "2") <= 0).AsQueryable();
          break;
      }
      foreach (var b in PDCID2)
      {
        ListID++;
        SortSequence++;
        res = new VFactoryIndexOC()
        {
          Id = ListID,
          Level = 2,
          ParentID = 1,
          UnitCode = b.PDC_ID,
          UnitName = b.PDC_Name,
          SortSeq = SortSequence,
          LineNum = null,
          RowCount = null,
        };
        result.Add(res);
        res = null;
      }

      #endregion

      res = null;
      SortSequence = 0;

      #region Get Level3

      var PDCID3 = (from T1 in ctx.TOrg
                    join T2 in PDCID2 on T1.PDC_ID equals T2.PDC_ID into nt
                    from T2 in nt.DefaultIfEmpty()
                    orderby T1.PDC_ID, T1.Building
                    select new
                    {
                      Factory_ID = T1.Factory_ID,
                      PDC_ID = T1.PDC_ID,
                      Building = T1.Building
                    }).Distinct().AsQueryable();

      //var PDCID3 = testL3.ToLookup(e => new { e.Factory_ID, e.PDC_ID, e.Building });
      //var PDCID3 = ctx.TOrg.Join(PDCID2, p => p.PDC_ID, q => q.PDC_ID, (p, q) => new { MES_Org = p, PDCID2 = q })
      //           .Select(p => new { p.MES_Org.Factory_ID, p.MES_Org.PDC_ID, p.MES_Org.Building })
      //           .Distinct().OrderBy(a => new { a.PDC_ID, a.Building }).AsQueryable();

      foreach (var b in PDCID3)
      {
        ListID++;
        SortSequence++;
        res = new VFactoryIndexOC()
        {
          Id = ListID,
          Level = 3,
          ParentID = result.Where(p => p.UnitCode == b.PDC_ID).Select(p => p.Id).FirstOrDefault(),
          UnitCode = b.Building,
          UnitName = b.Building,
          SortSeq = SortSequence,
          LineNum = null,
          RowCount = null,
        };
        result.Add(res);
        res = null;
      }

      #endregion

      res = null;
      SortSequence = 0;

      #region Get Level4
      //var test = PDCID3.AsQueryable();
      var PDCID4 = (from T1 in ctx.TOrg
                    join T2 in PDCID3 on new { T1.PDC_ID, T1.Building } equals new { T2.PDC_ID, T2.Building } into nt
                    //join T2 in PDCID3 on T1.PDC_ID equals T2.PDC_ID into nt
                    from T2 in nt.DefaultIfEmpty()
                    orderby T1.PDC_ID, T1.Building
                    select new
                    {
                      Factory_ID = T1.Factory_ID,
                      PDC_ID = T1.PDC_ID,
                      Building = T1.Building,
                      Line_ID = T1.Line_ID,
                      Line_Name = T1.Line_Name
                    }).Distinct().AsQueryable();
      //var PDCID4 = testL4.ToLookup(e => new { e.Factory_ID, e.PDC_ID, e.Building, e.Line_ID });
      //var PDCID4 = ctx.TOrg.Join(PDCID3, p => new { p.PDC_ID, p.Building }, q => new { q.PDC_ID, q.Building }, (p, q) => new { MES_Org = p, PDCID3 = q })
      //            .Select(p => new { p.MES_Org.Factory_ID, p.MES_Org.PDC_ID, p.MES_Org.Building, p.MES_Org.Line_ID })
      //            .Distinct().OrderByDescending(a => new { a.PDC_ID, a.Building }).ToList();

      foreach (var b in PDCID4)
      {
        ListID++;
        SortSequence++;
        res = new VFactoryIndexOC()
        {
          Id = ListID,
          Level = 4,
          ParentID = result.Where(p => p.UnitCode == b.Building).Select(p => p.Id).FirstOrDefault(),
          UnitCode = b.Line_ID,
          UnitName = b.Line_Name,
          SortSeq = SortSequence,
          LineNum = null,
          RowCount = 1,
        };
        result.Add(res);
        res = null;
      }

      #endregion

      #region Setting the LineNum for Level4 & 3

      int OneRowCellCount = 10;
      int intLineNum = 1;

      var L1 = result.Where(p => p.Level == 4).Select(p => p.ParentID).Distinct();
      var bbb = L1.Count();
      foreach (var lvl3ID in L1)
      {
        var a = lvl3ID.Value;
        int counter = 0;

        var L3 = result.Where(p => p.Id == lvl3ID.Value && p.Level == 3).FirstOrDefault();
        L3.LineNum = intLineNum.ToString().PadLeft(2, '0');

        var L2 = result.Where(p => p.Level == 4 && p.ParentID == lvl3ID.Value);
        foreach (var datal4 in L2)
        {
          counter++;
          if (counter > OneRowCellCount)
          {
            intLineNum++;
            counter = 1;
          }
          datal4.LineNum = intLineNum.ToString().PadLeft(2, '0');


        }
        intLineNum++;
      }

      #endregion

      #region Setting the LineNum for Level2

      var L4 = result.Where(p => p.Level == 2).Distinct();
      foreach (var lvl2Data in L4)
      {
        var firstLineNum = result.Where(p => p.ParentID == lvl2Data.Id && p.Level == 3).Select(p => p.LineNum).FirstOrDefault();
        lvl2Data.LineNum = firstLineNum;
      }

      #endregion

      #region Setting RowCount Level3,2,1

      var L5 = result.Where(p => p.Level == 3).Distinct();
      foreach (var lvl3Data in L5)
      {
        var distCountLineNum = result.Where(p => p.ParentID == lvl3Data.Id && p.Level == 4).Select(p => p.LineNum).Distinct().Count();
        lvl3Data.RowCount = distCountLineNum;
      }
      var L6 = result.Where(p => p.Level == 2).Distinct();
      foreach (var lvl2Data in L6)
      {
        var SumLineNumLvl3 = result.Where(p => p.ParentID == lvl2Data.Id && p.Level == 3).Select(p => p.RowCount).Sum();
        lvl2Data.RowCount = SumLineNumLvl3;
      }
      var L7 = result.Where(p => p.Level == 1).Distinct();
      foreach (var lvl1Data in L7)
      {
        var SumLineNumLvl2 = result.Where(p => p.ParentID == lvl1Data.Id && p.Level == 2).Select(p => p.RowCount).Sum();
        lvl1Data.RowCount = SumLineNumLvl2;
      }

      #endregion

      var a1 = result.Select(p => new { p.Id, p.Level, p.ParentID, p.UnitCode, p.UnitName, p.SortSeq, p.LineNum, p.RowCount }).ToList();

      // 判斷此線是否維修中
      var a2 = result.Where(d => d.Level == 4).ToList();
      foreach (var ii in a2)
      {
        // 看板數量比對站台table數量 相等:站台全報修(灰) 小於:部分站台報修(黃)
        var kanbancount = (from a in ctx.ITOfficeKanban
                           where a.FactoryID == FactoryID && a.Line_ID == ii.UnitCode && a.Line_Name == ii.UnitName &&
                           (a.IsRepaired == "N" || a.IsRepaired == "R")
                           select a).ToList();
        var linestationcount = (from a in ctx.LineStation
                                where a.Line_ID == ii.UnitCode
                                select a).ToList();

        if (kanbancount.Count == 0) // 該線沒有報修任何站台
        {
          ii.status = "";
        }
        else if (kanbancount.Count == linestationcount.Count) // 站台全報修(灰)
        {
          ii.status = "all";
        }
        else if (kanbancount.Count < linestationcount.Count) // 部分站台報修(黃)
        {
          ii.status = "part";
        }
        // var kanban = (from a in ctx.ITOfficeKanban
        //               where a.FactoryID == FactoryID && a.Line_ID == ii.UnitCode
        //               && a.Line_Name == ii.UnitName && a.IsRepaired == "N"
        //               select a).FirstOrDefault();
        // // 判斷狀態 R=維修中, N=開單等待中
        // if (kanban != null)
        // {
        //   ii.status = kanban.Status;
        // }
      }

      return result;

      #endregion
    }

    public async Task<bool> Add(ITOfficeKanbanDTO model)
    {
      var officekanban = _mapper.Map<ITOfficeKanban>(model);
      _itorgRepo.Add(officekanban);
      return await _itorgRepo.SaveAll();
    }

    public Task<bool> Update(ITOfficeKanbanDTO model)
    {
      throw new NotImplementedException();
    }

    // disable
    public Task<bool> Delete(ITOfficeKanbanDTO model)
    {
      throw new System.NotImplementedException();
    }
    // disable
    public Task<bool> UpdateForFinish(ITOfficeKanbanDTO model)
    {
      throw new System.NotImplementedException();
    }
  }
}