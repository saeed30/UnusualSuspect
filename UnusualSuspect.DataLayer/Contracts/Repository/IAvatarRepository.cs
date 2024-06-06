using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IAvatarRepository : IAsyncRepository<Avatar, short>
{
}