using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class GameGetResponse
  {
    public GameGetResponse(GameBaseDto gameBaseDto, GameFlowDto gameFlowDto, List<int> onlineUserIds)
    {
      GameBaseDto = gameBaseDto;
      GameFlowDto = gameFlowDto;
      RoomName = gameBaseDto.Id.ToString();
      OnlineUserIds = onlineUserIds;
    }

    [SerializeField]
    private GameBaseDto gameBaseDto;
    [SerializeField]
    private GameFlowDto gameFlowDto;
    [SerializeField]
    private string roomName;
    [SerializeField]
    private List<int> onlineUserIds;

    public List<int> OnlineUserIds
    {
      get => onlineUserIds;
      set => onlineUserIds = value;
    }

    public string RoomName
    {
      get => roomName;
      set => roomName = value;
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
