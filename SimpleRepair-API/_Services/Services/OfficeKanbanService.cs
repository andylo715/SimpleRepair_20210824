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

namespace SimpleRepair_API._Services.Services
{
  public class OfficeKanbanService : IOfficeKanbanService
  {
    private readonly IOfficeKanbanRepository _iofficekanbanRepo;
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configMapper;
    private string factory;

    public OfficeKanbanService(IOfficeKanbanRepository iofficekanbanRepo, IMapper mapper, MapperConfiguration configMapper, IConfiguration configuration)
    {
      _iofficekanbanRepo = iofficekanbanRepo;
      _mapper = mapper;
      _configMapper = configMapper;
      factory = configuration.GetSection("AppSettings:Factory").Value;
    }

    public async Task<PagedList<ITOfficeKanbanDTO>> SearchOfficeKanban(PaginationParams paginationParams, SearchParam searchParam)
    {
      var query = _iofficekanbanRepo.FindAll();
      if (searchParam.isrepaired == "N")
      {
        query = query.Where(x => x.IsRepaired.Contains(searchParam.isrepaired));
      }
      var list = query.ProjectTo<ITOfficeKanbanDTO>(_configMapper).OrderBy(x => x.SignRepairTime);
      return await PagedList<ITOfficeKanbanDTO>.CreateAsync(list, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public Task<bool> Add(ITOfficeKanbanDTO model)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> Update(ITOfficeKanbanDTO model)
    {
      var officekanban = _mapper.Map<ITOfficeKanban>(model);
      _iofficekanbanRepo.Update(officekanban);
      return await _iofficekanbanRepo.SaveAll();
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