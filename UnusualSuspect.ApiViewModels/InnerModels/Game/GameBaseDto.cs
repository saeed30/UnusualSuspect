using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class GameBaseDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private GameTypeDto gameTypeDto;
    [SerializeField]
    private List<GameParticipantDto> gameParticipantDto;
    [SerializeField]
    private List<GameCharacterDto> gameCharacterDtos;
    [SerializeField]
    private List<QuestionGameDto> questionGameDtos;
    [SerializeField]
    private PrivateInfoDto privateInfoDto;

    public PrivateInfoDto PrivateInfoDto
    {
      get => privateInfoDto;
      set => privateInfoDto = value;
    }

    public List<QuestionGameDto> QuestionGameDtos
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

    public List<GameParticipantDto> GameParticipantDto
    {
      get => gameParticipantDto;
      set => gameParticipantDto = value;
    }

    public List<GameCharacterDto> GameCharacterDtos
    {
      get => gameCharacterDtos;
      set => gameCharacterDtos = value;
    }
  }
}
