using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Question;

namespace UnusualSuspect.ViewModels.Mapper
{
  public static class QuestionCharacterCardDefaultAnswerMapper
  {
    public static GetFirstUnansweredQuestionViewmodel ToGetFirstUnansweredQuestionViewmodel(
      this QuestionCharacterCardDefaultAnswer model)
    {
      return new GetFirstUnansweredQuestionViewmodel()
      {
        CharacterCardId = model.CharacterCardId,
        CharacterCardImageUrl = model.CharacterCard.ImageUrl,
        QuestionContent = model.Question.QuestionContent,
        QuestionId = model.QuestionId
      };
    }
  }
}
