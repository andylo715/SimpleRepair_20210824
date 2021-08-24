using System.Threading.Tasks;
using SimpleRepair_API._Repositories.Interfaces;
using SimpleRepair_API.Data;
using SimpleRepair_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SimpleRepair_API._Repositories.Repositories
{
  public class LineStationRepository : MainRepository<LineStation>, ILineStationRepository
  {
    private readonly DataContext _content;

    public LineStationRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                            base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
    {
      _content = context;
    }

    public async Task<bool> CheckStationExists(string lineID, int stationID)
    {
      if (await _content.LineStation.AnyAsync(x => x.Line_ID == lineID && x.Station_ID == stationID))
      {
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}