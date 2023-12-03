using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IParticipateRepository : IAsyncRepository<Participate>
{
  Task<bool> IsGameParticipantAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<RoleCardEnum?> GetParticipantRoleAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<Participate?> GetActiveParticipation(int userId, CancellationToken cancellationToken = default);
}