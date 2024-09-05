using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGemPackageUserRepository : IAsyncRepository<GemPackageUser>
{
  Task<int> GetSumAsync(int userId);
  IQueryable<GemPackageUser> Search(GemPackageUserSearchFilterDto filterDto);
}