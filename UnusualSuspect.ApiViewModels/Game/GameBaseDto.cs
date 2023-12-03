using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Game
{
	[Serializable]
	public class GameBaseDto
	{
		[SerializeField]
		private int id;
		[SerializeField]
		private GameTypeDto gameTypeDto;
		[SerializeField]
		private IEnumerable<GameParticipantDto> gameParticipantDto;
		[SerializeField]
		private IEnumerable<GameCharacterDto> gameCharacterDtos;
		[SerializeField]
    private IEnumerable<QuestionGameDto> questionGameDtos;

    public IEnumerable<QuestionGameDto> QuestionGameDtos
    {
      get => questionGameDtos;
      set => questionGameDtos = value;
    }

    public int Id
		{
			get => id;
			set => id = value;
		}

		public GameTypeDto GameTypeDto
		{
			get => gameTypeDto;
			set => gameTypeDto = value;
		}

		public IEnumerable<GameParticipantDto> GameParticipantDto
		{
			get => gameParticipantDto;
			set => gameParticipantDto = value;
		}

		public IEnumerable<GameCharacterDto> GameCharacterDtos
		{
			get => gameCharacterDtos;
			set => gameCharacterDtos = value;
		}
	}
}
