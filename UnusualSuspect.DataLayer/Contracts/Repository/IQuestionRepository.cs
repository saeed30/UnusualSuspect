using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IQuestionRepository : IAsyncRepository<Question, short>
{
  Task<List<Question>> GetRandomActiveQuestionsAsync(int count, CancellationToken cancellationToken = default);
}