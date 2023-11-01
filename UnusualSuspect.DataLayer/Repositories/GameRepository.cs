using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class GameRepository : EfRepository<Game>, IGameRepository
{
	private readonly DbSet<Game> game;
	public GameRepository(IUnitOfWork uow, ILogger<GameRepository> logger) : base(uow, logger)
	{
		game = uow.Set<Game>();
	}

	public async Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default)
	{
		return await game.AsSplitQuery()
			.Include(x => x.CharacterCardGames)
			.ThenInclude(x => x.CharacterCard)
			.Include(x => x.GameType)
			.Include(c=>c.Participates)
			.Include(x=>x.QuestionGames)
			.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
	}
}