using AutoMapper;
using SimpleRepair_API.DTO;
using SimpleRepair_API.Models;

namespace SimpleRepair_API.Helpers.AutoMapper
{
  public class EfToDtoMappingProfile : Profile
  {
    public EfToDtoMappingProfile()
    {
      CreateMap<ITOfficeKanban, ITOfficeKanbanDTO>();
      CreateMap<TOrg, TOrgDTO>();
      CreateMap<LineStation,LineStationDTO>();
    }
  }
}