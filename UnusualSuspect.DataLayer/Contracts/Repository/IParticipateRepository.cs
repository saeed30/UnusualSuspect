using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IParticipateRepository : IAsyncRepository<Participate>
{
  Task<bool> IsGameParticipantAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<RoleCardEnum?> GetParticipantRoleAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<List<Participate>> GetActiveParticipations(int userId, CancellationToken cancellationToken = default);
  Task<int> GetParticipantCountAsync(int gameId, CancellationToken cancellationToken);
}