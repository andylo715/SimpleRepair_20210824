using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleRepair_API.DTO;
using SimpleRepair_API.Helpers;
using SimpleRepair_API.Models;

namespace SimpleRepair_API._Services.Interfaces
{
  public interface ITOrgService : IMainService<ITOfficeKanbanDTO>
  {
    List<VFactoryIndexOC> getFactoryIndexOC();
  }
}