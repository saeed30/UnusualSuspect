using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class TurnOfPlayGetResponse
  {
    [SerializeField]
    private bool isTalkingTime;
    [SerializeField]
    private TurnOfPlayTalkingState? turnOfPlayTalkingState;

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

    public bool IsTalkingTime
    {
      get => isTalkingTime;
      set => isTalkingTime = value;
    }

    public TurnOfPlayTalkingState? TurnOfPlayTalkingState
    {
      get => turnOfPlayTalkingState;
      set => turnOfPlayTalkingState = value;
    }
  }

  [Serializable]
  public class TurnOfPlayTalkingState
  {
    [SerializeField]
    private short orderOfParticipationTalkBeginner;
    [SerializeField]
    private short orderOfParticipationTurnToTalk;
    [SerializeField]
    private DateTime talkingTurnStartedTime;
    [SerializeField]
    private DateTime currentUserTurnStartedTime;
    [SerializeField]
    private Dictionary<int, short> candidateCard = new Dictionary<int, short>();

    public short OrderOfParticipationTalkBeginner
    {
      get => orderOfParticipationTalkBeginner;
      set => orderOfParticipationTalkBeginner = value;
    }

    public short OrderOfParticipationTurnToTalk
    {
      get => orderOfParticipationTurnToTalk;
      set => orderOfParticipationTurnToTalk = value;
    }

    public DateTime TalkingTurnStartedTime
    {
      get => talkingTurnStartedTime;
      set => talkingTurnStartedTime = value;
    }

    public DateTime CurrentUserTurnStartedTime
    {
      get => currentUserTurnStartedTime;
      set => currentUserTurnStartedTime = value;
    }

    public Dictionary<int, short> CandidateCard
    {
      get => candidateCard;
      set => candidateCard = value;
    }
  }
}
