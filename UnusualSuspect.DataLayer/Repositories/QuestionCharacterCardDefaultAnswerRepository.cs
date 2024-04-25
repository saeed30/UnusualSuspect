using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.ViewModels.Question;
using Dapper;
using UnusualSuspect.DataLayer.Contracts;

namespace UnusualSuspect.DataLayer.Repositories;
public sealed class QuestionCharacterCardDefaultAnswerRepository(IUnitOfWork uow,
    ILogger<QuestionCharacterCardDefaultAnswerRepository> logger,
    ApplicationDbContext context,
    IDapperRepository dapperRepository)
  : EfRepository<QuestionCharacterCardDefaultAnswer>(uow, logger), IQuestionCharacterCardDefaultAnswerRepository
{
  private readonly DbSet<QuestionCharacterCardDefaultAnswer> questionCharacterCardDefaultAnswer =
    uow.Set<QuestionCharacterCardDefaultAnswer>();
  public async Task<QuestionCharacterCardDefaultAnswer?> GetDefaultAnswer(short characterCardId, short questionId,
    CancellationToken cancellationToken = default)
  {
    return await questionCharacterCardDefaultAnswer
      .FirstOrDefaultAsync(x =>
      x.CharacterCardId == characterCardId && x.QuestionId == questionId, cancellationToken);
  }

  public IQueryable<QuestionCharacterCardDefaultAnswer> GetAllWithDetails()
  {
    return questionCharacterCardDefaultAnswer
      .Include(x => x.CharacterCard)
      .Include(x => x.Question);
  }

  public async Task<GetFirstUnansweredQuestionViewmodel?> GetFirstUnanswered(CancellationToken cancellationToken)
  {
    return await dapperRepository.QuerySingleAsync<GetFirstUnansweredQuestionViewmodel>(@"
select top(1) qc.QuestionId, qc.QuestionContent, qc.CharacterCardId, qc.ImageUrl as CharacterCardImageUrl from 
(select q.Id as QuestionId, q.QuestionContent, c.Id as CharacterCardId, c.ImageUrl from Question q cross join CharacterCard c 
where c.IsActive = 1 and q.IsActive = 1) as qc
left join QuestionCharacterCardDefaultAnswer d on qc.CharacterCardId = d.CharacterCardId and qc.QuestionId = d.QuestionId
where d.Id is null
order by qc.QuestionId, qc.CharacterCardId");
  }

  public async Task<List<QuestionCharacterCardDefaultAnswer>> GetByCharacterIdAndQuestionId(short characterCardId, short questionId,
    CancellationToken cancellationToken = default)
  {
    return await questionCharacterCardDefaultAnswer
      .Where(x => x.CharacterCardId == characterCardId && x.QuestionId == questionId).ToListAsync(cancellationToken);
  }
}
