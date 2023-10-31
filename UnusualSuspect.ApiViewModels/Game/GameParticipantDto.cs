using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.ApiViewModels.Game
{
	[Serializable]
	public class GameParticipantDto
	{
		[SerializeField]
		private int id;
		[SerializeField]
		private short orderOfParticipation;
		[SerializeField]
		private GameUserDto gameUserDto;
		[SerializeField]
		private GameRole gameRole;

		public int Id
		{
			get => id;
			set => id = value;
		}

		public short OrderOfParticipation
		{
			get => orderOfParticipation;
			set => orderOfParticipation = value;
		}

		public GameUserDto GameUserDto
		{
			get => gameUserDto;
			set => gameUserDto = value;
		}

		public GameRole GameRole
		{
			get => gameRole;
			set => gameRole = value;
		}
	}
}
