using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
  public interface IQuestionRepository : IAsyncRepository<Question, short>
  {
  }
}
