using System.Threading.Tasks;
using SimpleRepair_API._Repositories.Interfaces;
using SimpleRepair_API.Data;
using SimpleRepair_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SimpleRepair_API._Repositories.Repositories
{
  public class TOrgRepository : MainRepository<ITOfficeKanban>, ITOrgRepository
  {
    private readonly DataContext _content;

    public TOrgRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
     base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
    {
      _content = context;
    }
  }
}