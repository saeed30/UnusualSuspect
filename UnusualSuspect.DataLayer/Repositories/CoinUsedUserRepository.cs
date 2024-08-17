using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Model;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class CoinUsedUserRepository(IUnitOfWork uow, ILogger<CoinUsedUserRepository> logger)
  : EfRepository<CoinUsedUser>(uow, logger), ICoinUsedUserRepository
{
  public async Task<int> GetSumAsync(int userId)
  {
    return await BaseEntity.Where(x => x.UserId == userId).SumAsync(x => x.Amount);
  }

  public Task<CoinUsedUser?> GetPreGameSavePaymentAsync(Guid referenceGuid, CancellationToken cancellationToken = default)
  {
    return BaseEntity.FirstOrDefaultAsync(x =>
      x.ReferenceGuid == referenceGuid && x.UsedForPriceTypeId == (int)PriceTypeEnum.PreGame, cancellationToken);
  }

  public IQueryable<CoinUsedUser> Search(CoinUsedUserSearchFilterDto filterDto)
  {
    IQueryable<CoinUsedUser> result = BaseEntity;
    if (filterDto.UserId.HasValue)
      result = result.Where(x => x.UserId == filterDto.UserId.Value);
    if (filterDto.Amount.HasValue)
      result = result.Where(x => x.Amount == filterDto.Amount.Value);
    if (filterDto.ReferenceGuid.HasValue)
      result = result.Where(x => x.ReferenceGuid == filterDto.ReferenceGuid.Value);
    if (filterDto.UsedForPriceType.HasValue)
      result = result.Where(x => x.UsedForPriceTypeId == (int)filterDto.UsedForPriceType.Value);
    return result;
  }
}