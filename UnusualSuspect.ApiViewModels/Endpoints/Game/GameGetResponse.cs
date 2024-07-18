using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class GameGetResponse
  {
    public GameGetResponse(GameBaseDto gameBaseDto, GameFlowDto gameFlowDto, List<int> onlineUserIds,
      short gameStatusId, PrivateInfoDto privateInfoDto, string? cachedTimeString, int baseTimingInSeconds)
    {
      GameBaseDto = gameBaseDto;
      GameFlowDto = gameFlowDto;
      RoomName = gameBaseDto.Id.ToString();
      OnlineUserIds = onlineUserIds;
      GameStatusId = gameStatusId;
      PrivateInfoDto = privateInfoDto;
      CachedTimeString = cachedTimeString;
      ServerCurrentTimeString = DateTime.Now.ToString();
      BaseTimingInSeconds = baseTimingInSeconds;
    }

    [SerializeField]
    private GameBaseDto gameBaseDto;
    [SerializeField]
    private GameFlowDto gameFlowDto;
    [SerializeField]
    private string roomName;
    [SerializeField]
    private List<int> onlineUserIds;
    [SerializeField]
    private short gameStatusId;
    [SerializeField]
    private PrivateInfoDto privateInfoDto;
    [SerializeField]
    private string? cachedTimeString;
    [SerializeField]
    private int baseTimingInSeconds;
    [SerializeField]
    private string serverCurrentTimeString;

    public string ServerCurrentTimeString
    {
      get => serverCurrentTimeString;
      set => serverCurrentTimeString = value;
    }

    public int BaseTimingInSeconds
    {
      get => baseTimingInSeconds;
      set => baseTimingInSeconds = value;
    }


    public string? CachedTimeString
    {
      get => cachedTimeString;
      set => cachedTimeString = value;
    }

    public PrivateInfoDto PrivateInfoDto
    {
      get => privateInfoDto;
      set => privateInfoDto = value;
    }

    public short GameStatusId
    {
      get => gameStatusId;
      set => gameStatusId = value;
    }

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
