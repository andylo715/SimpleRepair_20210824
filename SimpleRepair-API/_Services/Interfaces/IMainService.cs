using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleRepair_API.Helpers;

namespace SimpleRepair_API._Services.Interfaces
{
  public interface IMainService<T> where T : class
  {
    Task<bool> Add(T model);

    Task<bool> Update(T model);

    Task<bool> Delete(T model);
  }
}