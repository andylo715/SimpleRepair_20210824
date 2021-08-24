using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleRepair_API.DTO;
using SimpleRepair_API.Helpers;

namespace SimpleRepair_API._Services.Interfaces
{
  public interface IOfficeKanbanService : IMainService<ITOfficeKanbanDTO>
  {
    Task<PagedList<ITOfficeKanbanDTO>> SearchOfficeKanban(PaginationParams paginationParams, SearchParam isrepaired);

  }
}