using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
  public interface ICoinUsedUserRepository : IAsyncRepository<CoinUsedUser>
  {
    Task<int> GetSumAsync(int userId);
  }
}
