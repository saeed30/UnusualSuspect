using System;
using System.Collections.Generic;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  public class TurnOfPlayGetResponse
  {
    public TurnOfPlayGetResponse()
    {
      TurnOfPlayTalkingState = null;
      IsTalkingTime = false;
    }
    public TurnOfPlayGetResponse(TurnOfPlayTalkingState turnOfPlayTalkingState)
    {
      TurnOfPlayTalkingState = turnOfPlayTalkingState;
      IsTalkingTime = true;
    }
    public bool IsTalkingTime { get; set; }
    public TurnOfPlayTalkingState? TurnOfPlayTalkingState { get; set; }
  }

  public class TurnOfPlayTalkingState
  {
    public short OrderOfParticipationTalkBeginner { get; set; }
    public short OrderOfParticipationTurnToTalk { get; set; }
    public DateTime TalkingTurnStartedTime { get; set; }
    public DateTime CurrentUserTurnStartedTime { get; set; }
  }
}
