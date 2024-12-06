using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IParticipateRepository : IAsyncRepository<Participate>
{
  Task<bool> IsGameParticipantAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<RoleCardEnum?> GetParticipantRoleAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<List<Participate>> GetActiveParticipations(int userId, CancellationToken cancellationToken = default);
  Task<int> GetParticipantCountAsync(int gameId, CancellationToken cancellationToken = default);
  Task<List<Participate>> GetGameActiveParticipantsAsync(int gameId, RoleCardEnum? userRole = null, CancellationToken cancellationToken = default);
  Task<bool> IsGameHasOtherActiveParticipantsAsync(int gameId, IEnumerable<int> userIds);
  IQueryable<int> GetUsersGameIds(List<int> userIds);
}