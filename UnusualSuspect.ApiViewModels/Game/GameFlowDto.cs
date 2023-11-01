using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Game
{
	[Serializable]
	public class GameFlowDto
	{
		[SerializeField]
		private int id;
		[SerializeField]
		private List<int> activeCharacterIds;
		[SerializeField]
		private List<int> usedQuestionIds;

		public int Id
		{
			get => id;
			set => id = value;
		}

		public List<int> ActiveCharacterIds
		{
			get => activeCharacterIds;
			set => activeCharacterIds = value;
		}

		public List<int> UsedQuestionIds
		{
			get => usedQuestionIds;
			set => usedQuestionIds = value;
		}
	}
}
