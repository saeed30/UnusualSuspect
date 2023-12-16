using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class GameGetResponse
  {
    [SerializeField]
    private GameBaseDto gameBaseDto;
    [SerializeField]
    private GameFlowDto gameFlowDto;
    [SerializeField]
    private string roomName;

    public string RoomName
    {
      get => roomName;
      set => roomName = value;
    }

    public GameGetResponse(GameBaseDto gameBaseDto, GameFlowDto gameFlowDto)
    {
      GameBaseDto = gameBaseDto;
      GameFlowDto = gameFlowDto;
      RoomName = gameBaseDto.Id.ToString();
    }

    public GameBaseDto GameBaseDto
    {
      get => gameBaseDto;
      set => gameBaseDto = value;
    }

    public GameFlowDto GameFlowDto
    {
      get => gameFlowDto;
      set => gameFlowDto = value;
    }
  }
}
