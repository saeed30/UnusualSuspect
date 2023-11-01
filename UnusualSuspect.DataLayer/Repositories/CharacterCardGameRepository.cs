using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class CharacterCardGameRepository: EfRepository<CharacterCardGame>, ICharacterCardGameRepository
{
	public CharacterCardGameRepository(IUnitOfWork uow, ILogger<CharacterCardGameRepository> logger) : base(uow, logger)
	{
	}

}