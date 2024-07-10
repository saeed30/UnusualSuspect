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
      short gameStatusId, PrivateInfoDto privateInfoDto, DateTime? cachedTime, int baseTimingInSeconds)
    {
      GameBaseDto = gameBaseDto;
      GameFlowDto = gameFlowDto;
      RoomName = gameBaseDto.Id.ToString();
      OnlineUserIds = onlineUserIds;
      GameStatusId = gameStatusId;
      PrivateInfoDto = privateInfoDto;
      CachedTime = cachedTime;
      ServerCurrentTime = DateTime.Now;
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
    private DateTime? cachedTime;
    [SerializeField]
    private int baseTimingInSeconds;
    [SerializeField]
    private DateTime serverCurrentTime;

    public DateTime ServerCurrentTime
    {
      get => serverCurrentTime;
      set => serverCurrentTime = value;
    }

    public int BaseTimingInSeconds
    {
      get => baseTimingInSeconds;
      set => baseTimingInSeconds = value;
    }


    public DateTime? CachedTime
    {
      get => cachedTime;
      set => cachedTime = value;
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
