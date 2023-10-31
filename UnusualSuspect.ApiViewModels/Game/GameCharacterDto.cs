using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Game
{
	[Serializable]
	public class GameCharacterDto
	{
		[SerializeField]
		private int id;
		[SerializeField]
		private short characterId;
		[SerializeField]
		private string title;
		[SerializeField]
		private bool isMurderer;

		public int Id
		{
			get => id;
			set => id = value;
		}

		public short CharacterId
		{
			get => characterId;
			set => characterId = value;
		}

		public string Title
		{
			get => title;
			set => title = value;
		}

		public bool IsMurderer
		{
			get => isMurderer;
			set => isMurderer = value;
		}
	}
}
