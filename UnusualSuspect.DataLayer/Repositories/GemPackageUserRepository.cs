using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GemPackageUserRepository(IUnitOfWork uow, ILogger<GemPackageUserRepository> logger)
  : EfRepository<GemPackageUser>(uow, logger), IGemPackageUserRepository
{
  public async Task<int> GetSumAsync(int userId)
  {
    return await BaseEntity.Where(x => x.UserId == userId && x.IsActive).SumAsync(x => x.Amount);
  }

  public IQueryable<GemPackageUser> Search(GemPackageUserSearchFilterDto filterDto)
  {
    IQueryable<GemPackageUser> result = BaseEntity;
    if (filterDto.UserId.HasValue)
      result = result.Where(x => x.UserId == filterDto.UserId.Value);
    if (filterDto.OnlyToday)
      result = result.Where(x => x.TimeAdded.Date == DateTime.Now.Date);
    if (filterDto.PackageId.HasValue)
      result = result.Where(x => x.GemPackageId == filterDto.PackageId.Value);
    return result;
  }
}