using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Question;

namespace UnusualSuspect.DataLayer.Contracts.Repository;
public interface IQuestionCharacterCardDefaultAnswerRepository : IAsyncRepository<QuestionCharacterCardDefaultAnswer>
{
  Task<QuestionCharacterCardDefaultAnswer?> GetDefaultAnswer(short characterCardId, short questionId, CancellationToken cancellationToken = default);
  IQueryable<QuestionCharacterCardDefaultAnswer> GetAllWithDetails();
  Task<GetFirstUnansweredQuestionViewmodel?> GetFirstUnanswered(CancellationToken cancellationToken = default);
  Task<List<QuestionCharacterCardDefaultAnswer>> GetByCharacterIdAndQuestionId(short characterCardId, short questionId, CancellationToken cancellationToken = default);
}
