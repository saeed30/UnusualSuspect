using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class CoinPackageUserRepository(IUnitOfWork uow, ILogger<CoinPackageUserRepository> logger)
  : EfRepository<CoinPackageUser>(uow, logger), ICoinPackageUserRepository
{
  public async Task<int> GetSumAsync(int userId)
  {
    return await BaseEntity.Where(x => x.UserId == userId && x.IsActive).SumAsync(x => x.Amount);
  }
  public IQueryable<CoinPackageUser> Search(CoinPackageUserSearchFilterDto filterDto)
  {
    IQueryable<CoinPackageUser> result = BaseEntity;
    if (filterDto.UserId.HasValue)
      result = result.Where(x => x.UserId == filterDto.UserId.Value);
    if (filterDto.OnlyToday)
      result = result.Where(x => x.TimeAdded.Date == DateTime.Now.Date);
    if (filterDto.PackageId.HasValue)
      result = result.Where(x => x.CoinPackageId == filterDto.PackageId.Value);
    return result;
  }
}