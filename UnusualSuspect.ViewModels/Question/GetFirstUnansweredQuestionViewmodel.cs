using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Question
{
  public sealed class GetFirstUnansweredQuestionViewmodel
  {
  public short QuestionId { get; set; }
  public string QuestionContent { get; set; }
  public short CharacterCardId { get; set; }
  public string CharacterCardImageUrl { get; set; }
  }
}
