using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GameRepository(IUnitOfWork uow, ILogger<GameRepository> logger)
  : EfRepository<Game>(uow, logger), IGameRepository
{
  private readonly DbSet<Game> games = uow.Set<Game>();

  public async Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default)
  {
    return await games.AsSplitQuery()
      .Include(x => x.CharacterCardGames)
      .ThenInclude(x => x.CharacterCard)
      .Include(x => x.GameType)
      .Include(c => c.Participates)
      .Include(x => x.QuestionGames)
      .ThenInclude(x => x.Question)
      .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
  }

  public async Task<bool> SetGameFinishTimeAsync(int id, DateTime finishTime, CancellationToken cancellationToken = default)
  {
    Game? g = await GetByIdAsync(id, cancellationToken);
    if (g == null)
      return false;
    g.FinishedTime = finishTime;
    return true;
  }

  public async Task<bool> SetGameWinStateAsync(int id, bool won, CancellationToken cancellationToken = default)
  {
    var game = await GetByIdAsync(id, cancellationToken);
    if (game == null) return false;
    game.WonTheGame = won;
    return true;
  }

  public IQueryable<Game> GetAllActiveGamesWithGameType()
  {
    return games
      .Include(x => x.GameType)
      .Where(x => x.FinishedTime == null);
  }

  public async Task<Game?> GetUserCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await games.FirstOrDefaultAsync(
      x => x.FinishedTime == null &&
           x.Participates.Any(p => p.UserId == userId), cancellationToken);
    if (game == null)
      return null;
    return await GetGameWithDetailsAsync(game.Id, cancellationToken);
  }
}