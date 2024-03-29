using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
	[Serializable]
	public class TurnOfPlayGetResponse
	{
		[SerializeField]
		private TurnOfPlayTalkingState? turnOfPlayTalkingState;

		public TurnOfPlayGetResponse()
		{
			TurnOfPlayTalkingState = null;
		}
		public TurnOfPlayGetResponse(TurnOfPlayTalkingState? turnOfPlayTalkingState)
		{
			if (turnOfPlayTalkingState == null)
			{
				TurnOfPlayTalkingState = null;
			}
			else
			{
				TurnOfPlayTalkingState = turnOfPlayTalkingState;
			}
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
		private int timeToTalkInSeconds;
		[SerializeField]
		private short orderOfParticipationTalkBeginner;
		[SerializeField]
		private short orderOfParticipationTurnToTalk;
		[SerializeField]
		private DateTime talkingTurnStartedTime;
		[SerializeField]
		private DateTime currentUserTurnStartedTime;

		public int TimeToTalkInSeconds
		{
			get => timeToTalkInSeconds;
			set => timeToTalkInSeconds = value > 0 ? value : 0;
		}

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

	}
}
