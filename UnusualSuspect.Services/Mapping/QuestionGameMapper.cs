using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class QuestionGameMapper
{
  public static QuestionGameDto ToQuestionGameDto(this QuestionGame value)
  {
    return new QuestionGameDto()
    {
      QuestionContent = value.Question.QuestionContent,
      QuestionGameId = value.Id,
      QuestionId = value.QuestionId,
      Turn = value.Turn,
      WithnessAnswer = value.UserAnswer
    };
  }
  public static List<QuestionGameDto> ToQuestionGameDto(this ICollection<QuestionGame> value)
  {
    return value.Select(x => x.ToQuestionGameDto()).ToList();
  }

}