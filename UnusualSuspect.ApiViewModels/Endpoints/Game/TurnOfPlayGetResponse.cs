using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
	[Serializable]
	public class TurnOfPlayGetResponse
	{
		[SerializeField]
		private TurnOfPlayTalkingState? turnOfPlayTalkingState;
    [SerializeField]
    private DateTime currentUserTurnStartedTime;

    public TurnOfPlayGetResponse()
		{
			TurnOfPlayTalkingState = null;
      currentUserTurnStartedTime = DateTime.MinValue;
    }
		public TurnOfPlayGetResponse(TurnOfPlayTalkingState? turnOfPlayTalkingState,
      DateTime? currentUserTurnStartedTime)
    {
      CurrentUserTurnStartedTime = currentUserTurnStartedTime??DateTime.MinValue;
      if (turnOfPlayTalkingState == null)
			{
				TurnOfPlayTalkingState = null;
			}
			else
			{
				TurnOfPlayTalkingState = turnOfPlayTalkingState;
			}
		}
    public DateTime CurrentUserTurnStartedTime
    {
      get => currentUserTurnStartedTime;
      set => currentUserTurnStartedTime = value;
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
	}
}
