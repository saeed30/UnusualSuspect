using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
  [Serializable]
  public class PreGameGroupGetResponse
  {
    [SerializeField]
    private int preGameGroupId;
    [SerializeField]
    private DateTime createdTime;
    [SerializeField]
    private short calculatedJoinedUsers;
    [SerializeField]
    private GameTypeDto gameTypeDto;
    [SerializeField]
    private short preGameGroupStatusId;
    [SerializeField]
    private int? gameId;
    [SerializeField]
    private List<JoinedPreGameDto> joinedPreGame = new List<JoinedPreGameDto>();

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

    public List<JoinedPreGameDto> JoinedPreGame
    {
      get => joinedPreGame;
      set => joinedPreGame = value;
    }
  }
}
