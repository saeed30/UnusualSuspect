using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IQuestionGameRepository : IAsyncRepository<QuestionGame>
{
	Task<QuestionGame?> GetQuestionGameByQuestionAndGame(int gameId, short questionId);
}