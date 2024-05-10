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
    private string createdTimeString;
    [SerializeField]
    private string? readyToGameTimeString;
    [SerializeField]
    private short calculatedJoinedUsers;
    [SerializeField]
    private GameTypeDto gameTypeDto;
    [SerializeField]
    private short preGameGroupStatusId;
    [SerializeField]
    private int? gameId;
    [SerializeField]
    private UserDto ownerDto;

    public UserDto OwnerDto
    {
      get => ownerDto;
      set => ownerDto = value;
    }

    public int PreGameGroupId
    {
      get => preGameGroupId;
      set => preGameGroupId = value;
    }

    public string CreatedTimeString
    {
      get => createdTimeString;
      set => createdTimeString = value;
    }

    public string? ReadyToGameTimeString
    {
      get => readyToGameTimeString;
      set => readyToGameTimeString = value;
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
