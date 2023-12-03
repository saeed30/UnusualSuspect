using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping
{
  public static class QuestionGameMapper
  {
    public static QuestionGameDto ToQuestionGameDto(this QuestionGame value)
    {
      return new QuestionGameDto()
      {
        QuestionContent = value.Question.QuestionContent,
        QuestionGameId = value.Id,
        QuestionId = value.QuestionId,
        Turn = value.Turn
      };
    }
    public static IEnumerable<QuestionGameDto> ToQuestionGameDto(this IEnumerable<QuestionGame> value)
    {
      return value.Select(x => x.ToQuestionGameDto());
    }

  }
}
