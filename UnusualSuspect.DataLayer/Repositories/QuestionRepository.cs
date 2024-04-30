using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class QuestionRepository(IUnitOfWork uow, ILogger<QuestionRepository> logger)
  : EfRepository<Question, short>(uow, logger), IQuestionRepository
{
  public async Task<List<Question>> GetRandomActiveQuestionsAsync(int count, CancellationToken cancellationToken = default)
  {
    if (count < 1)
      throw new Exception("count should be more than 0");
    return await baseEntity.Where(x => x.IsActive).OrderBy(r => Guid.NewGuid()).Take(count).ToListAsync(cancellationToken);
  }
}