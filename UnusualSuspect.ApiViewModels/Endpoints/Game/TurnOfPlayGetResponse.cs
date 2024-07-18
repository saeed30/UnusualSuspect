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
    private string currentUserTurnStartedTimeString;

    public TurnOfPlayGetResponse()
		{
			TurnOfPlayTalkingState = null;
      currentUserTurnStartedTimeString = DateTime.MinValue.ToString();
    }
		public TurnOfPlayGetResponse(TurnOfPlayTalkingState? turnOfPlayTalkingState,
      string? currentUserTurnStartedTimeString)
    {
      CurrentUserTurnStartedTimeString = currentUserTurnStartedTimeString ??DateTime.MinValue.ToString();
      if (turnOfPlayTalkingState == null)
			{
				TurnOfPlayTalkingState = null;
			}
			else
			{
				TurnOfPlayTalkingState = turnOfPlayTalkingState;
			}
		}
    public string CurrentUserTurnStartedTimeString
    {
      get => currentUserTurnStartedTimeString;
      set => currentUserTurnStartedTimeString = value;
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
		private string talkingTurnStartedTimeString;

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

		public string TalkingTurnStartedTimeString
		{
			get => talkingTurnStartedTimeString;
			set => talkingTurnStartedTimeString = value;
		}
	}
}
