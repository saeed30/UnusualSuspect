using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class CharacterCardRepository
  (IUnitOfWork uow,
    ILogger<CharacterCardRepository> logger,
    IDapperRepository dapperRepository)
  : EfRepository<CharacterCard, short>(uow, logger),
    ICharacterCardRepository
{
  public IQueryable<CharacterCard> GetAllCharacterCards()
  {
    return baseEntity.Select(x => new CharacterCard()
    {
      Id = x.Id,
      IsActive = x.IsActive,
      ImageUrl = x.ImageUrl,
      OriginalId = x.Id,
      Title = x.Title
    });
  }

  public async Task ExecuteUpdateByIdAsync(CharacterCard characterCard, short originalId, CancellationToken cancellationToken = default)
  {
    await baseEntity.Where(x => x.Id == originalId).ExecuteUpdateAsync(x => x
        .SetProperty(a => a.ImageUrl, a => characterCard.ImageUrl)
        .SetProperty(a => a.Id, a => characterCard.Id)
        .SetProperty(a => a.IsActive, a => characterCard.IsActive)
        .SetProperty(a => a.Title, a => characterCard.Title)
      , cancellationToken);
  }
  public async Task<List<CharacterCard>> GetAllActiveCharacterCardsAsync(CancellationToken cancellationToken = default)
  {
    return await baseEntity.Where(x => x.IsActive).ToListAsync(cancellationToken);
  }

  public async Task<List<CharacterCard>> GetRandomActiveCharacterCardsAsync(int count, CancellationToken cancellationToken = default)
  {
    if (count < 1)
      throw new Exception("count should be more than 0");
    return await baseEntity.Where(x => x.IsActive).OrderBy(r => Guid.NewGuid()).Take(count).ToListAsync(cancellationToken);
  }
}