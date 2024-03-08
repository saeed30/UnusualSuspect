using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IStickerRepository : IAsyncRepository<Sticker, short>
{
}