using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.ApiViewModels.InnerModels.PreGame
{
  [Serializable]
  public sealed class PreGameGroupDto
  {
    [SerializeField]
    private int preGameGroupId;
    [SerializeField]
    private DateTime createdTime;
    [SerializeField]
    private DateTime? readyToGameTime;
    [SerializeField]
    private short calculatedJoinedUsers;
    [SerializeField]
    private GameTypeDto gameTypeDto;
    [SerializeField]
    private short preGameGroupStatusId;
    [SerializeField]
    private int? gameId;

    public int PreGameGroupId
    {
      get => preGameGroupId;
      set => preGameGroupId = value;
    }

    public DateTime CreatedTime
    {
      get => createdTime;
      set => createdTime = value;
    }

    public DateTime? ReadyToGameTime
    {
      get => readyToGameTime;
      set => readyToGameTime = value;
    }

    public short CalculatedJoinedUsers
    {
      get => calculatedJoinedUsers;
      set => calculatedJoinedUsers = value;
    }

    public GameTypeDto GameTypeDto
    {
      get => gameTypeDto;
      set => gameTypeDto = value;
    }

    public short PreGameGroupStatusId
    {
      get => preGameGroupStatusId;
      set => preGameGroupStatusId = value;
    }

    public int? GameId
    {
      get => gameId;
      set => gameId = value;
    }
  }
}
