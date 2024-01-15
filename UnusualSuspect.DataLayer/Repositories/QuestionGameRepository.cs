using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class QuestionGameRepository(IUnitOfWork uow, ILogger<QuestionGameRepository> logger)
  : EfRepository<QuestionGame>(uow, logger), IQuestionGameRepository
{
  private readonly DbSet<QuestionGame> questionGame = uow.Set<QuestionGame>();

  public async Task<QuestionGame?> GetQuestionGameByQuestionAndGame(int gameId, short questionId)
  {
	  return await questionGame.FirstOrDefaultAsync(x=>x.GameId == gameId && x.QuestionId == questionId);

  }
}