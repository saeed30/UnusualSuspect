using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Question;

namespace UnusualSuspect.Services.Contracts;
public interface IQuestionService
{
  Task<UnusualSuspectServiceResult<bool>> GetDefaultAnswer(short characterCardId, short  questionId,
    CancellationToken cancellationToken = default);
  IQueryable<QuestionCharacterCardDefaultAnswer> GetAllDefaultAnswersWithDetails();
  IQueryable<Question> GetAllQuestions(bool? isActive = null);

  Task<GetFirstUnansweredQuestionViewmodel?> GetFirstUnanswered(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> SetDefaultAnswer(SetQuestionDefaultAnswerViewmodel model, CancellationToken cancellationToken = default);
  void DeleteQuestionCharacterCardDefaultAnswer(int id);
  void AddQuestion(Question question);
  void UpdateQuestion(Question question);
  void DeleteQuestionById(short questionId);
}
