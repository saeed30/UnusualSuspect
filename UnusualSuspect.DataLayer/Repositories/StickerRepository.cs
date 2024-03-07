using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class StickerRepository
  (IUnitOfWork uow, ILogger<StickerRepository> logger) : EfRepository<Sticker, short>(uow, logger),
    IStickerRepository
{
}