using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICoinUsedUserRepository : IAsyncRepository<CoinUsedUser>
{
  Task<int> GetSumAsync(int userId);
  Task<CoinUsedUser?> GetPreGameSavePaymentAsync(Guid referenceGuid, CancellationToken cancellationToken = default);
  IQueryable<CoinUsedUser> Search(CoinUsedUserSearchFilterDto filterDto);
}