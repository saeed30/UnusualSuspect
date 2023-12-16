using System;
using UnityEngine;

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
    private short gameTypeId;
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

    public short GameTypeId
    {
      get => gameTypeId;
      set => gameTypeId = value;
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
