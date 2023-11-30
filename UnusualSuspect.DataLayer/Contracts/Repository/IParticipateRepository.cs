using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IParticipateRepository : IAsyncRepository<Participate>
{
  Task<bool> IsGameParticipant(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<RoleCardEnum?> GetParticipantRole(int gameId, int userId, CancellationToken cancellationToken = default);
}