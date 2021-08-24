using System.Threading.Tasks;
using SimpleRepair_API.Models;

namespace SimpleRepair_API._Repositories.Interfaces
{
  public interface ILineStationRepository : IMainRepository<LineStation>
  {
    Task<bool> CheckStationExists(string lineID, int stationID);
  }
}