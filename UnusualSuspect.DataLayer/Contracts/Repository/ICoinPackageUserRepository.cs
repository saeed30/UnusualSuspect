using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICoinPackageUserRepository : IAsyncRepository<CoinPackageUser>
{
  Task<int> GetSumAsync(int userId);
  IQueryable<CoinPackageUser> Search(CoinPackageUserSearchFilterDto filterDto);
}