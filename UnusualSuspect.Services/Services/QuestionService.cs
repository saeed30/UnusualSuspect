using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Question;

namespace UnusualSuspect.Services.Services;
public class QuestionService(
    IQuestionCharacterCardDefaultAnswerRepository questionCharacterCardDefaultAnswerRepository,
    IQuestionRepository questionRepository,
    ICharacterCardRepository cardRepository)
  : IQuestionService
{
  public async Task<UnusualSuspectServiceResult<bool>> GetDefaultAnswer(short characterCardId, short questionId, CancellationToken cancellationToken = default)
  {
    QuestionCharacterCardDefaultAnswer? result = await questionCharacterCardDefaultAnswerRepository
      .GetDefaultAnswer(characterCardId, questionId, cancellationToken);
    if (result == null)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.NoDefaultAnswerForTheQuestion));
    return new UnusualSuspectServiceResult<bool>(result.DefaultAnswer);
  }

  public IQueryable<QuestionCharacterCardDefaultAnswer> GetAllDefaultAnswersWithDetails()
  {
    return questionCharacterCardDefaultAnswerRepository.GetAllWithDetails();
  }

  public async Task<GetFirstUnansweredQuestionViewmodel?> GetFirstUnanswered(CancellationToken cancellationToken = default)
  {
    return await questionCharacterCardDefaultAnswerRepository.GetFirstUnanswered(cancellationToken);
  }

  public async Task<UnusualSuspectServiceResult<bool>> SetDefaultAnswer(SetQuestionDefaultAnswerViewmodel model, CancellationToken cancellationToken = default)
  {
    if (await questionRepository.GetByIdAsync(model.QuestionId, cancellationToken) == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidQuestionId));
    if (await cardRepository.GetByIdAsync(model.CharacterCardId, cancellationToken) == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidCharacterCardId));
    List<QuestionCharacterCardDefaultAnswer> result =
      await questionCharacterCardDefaultAnswerRepository.GetByCharacterIdAndQuestionId(model.CharacterCardId, model.QuestionId, cancellationToken);
    if (!result.Any())
    {
      questionCharacterCardDefaultAnswerRepository.Add(
        new QuestionCharacterCardDefaultAnswer()
        {
          CharacterCardId = model.CharacterCardId,
          QuestionId = model.QuestionId,
          DateTimeAdded = DateTime.Now,
          DefaultAnswer = model.DefaultAnswer
        });
      return new UnusualSuspectServiceResult<bool>(true);
    }
    if (result.Count == 1)
    {
      if (result[0].DefaultAnswer == model.DefaultAnswer)
        return new UnusualSuspectServiceResult<bool>(false);
      result[0].DefaultAnswer = model.DefaultAnswer;
      return new UnusualSuspectServiceResult<bool>(true);
    }
    questionCharacterCardDefaultAnswerRepository.DeleteRange(result);
    questionCharacterCardDefaultAnswerRepository.Add(
      new QuestionCharacterCardDefaultAnswer()
      {
        CharacterCardId = model.CharacterCardId,
        QuestionId = model.QuestionId,
        DateTimeAdded = DateTime.Now,
        DefaultAnswer = model.DefaultAnswer
      });
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public void DeleteQuestionCharacterCardDefaultAnswer(int id)
  {
    questionCharacterCardDefaultAnswerRepository.DeleteById(id);
  }
}
