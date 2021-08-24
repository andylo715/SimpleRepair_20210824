using AutoMapper;
using SimpleRepair_API.DTO;
using SimpleRepair_API.Models;

namespace SimpleRepair_API.Helpers.AutoMapper
{
  public class DtoToEfMappingProfile : Profile
  {
    public DtoToEfMappingProfile()
    {
        CreateMap<ITOfficeKanbanDTO,ITOfficeKanban>();
        CreateMap<TOrgDTO,TOrg>();
        CreateMap<LineStationDTO,LineStation>();
    }
  }
}