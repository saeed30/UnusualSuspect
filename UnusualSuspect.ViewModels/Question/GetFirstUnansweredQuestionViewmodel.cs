namespace UnusualSuspect.ViewModels.Question;

public sealed class GetFirstUnansweredQuestionViewmodel
{
  public short QuestionId { get; set; }
  public string QuestionContent { get; set; }
  public short CharacterCardId { get; set; }
  public string CharacterCardImageUrl { get; set; }
}