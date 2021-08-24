using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleRepair_API.DTO;
using SimpleRepair_API.Helpers;
using SimpleRepair_API.Models;

namespace SimpleRepair_API._Services.Interfaces
{
  public interface ILineStationService : IMainService<LineStationDTO>
  {
    Task<PagedList<LineStationDTO>> SearchLineStation(PaginationParams paginationParams, StationSearchParam stationName);
    Task<object> GetAllLine();
    Task<bool> CheckStationExists(string lineID, int stationID);
    List<LineStation> getLineStationName(string lineID);
    LineStation getLineOneStation(string lineID, string stationname);
  }
}