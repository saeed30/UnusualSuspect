using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class FinishedResponse
  {
    [SerializeField]
    private TimeSpan sessionTimeSpan;
    [SerializeField]
    private int murdererId;
    [SerializeField]
    private List<FinishedParticipantDto> finishedParticipantDtos;
    [SerializeField]
    private bool won;

    public bool Won
    {
      get => won;
      set => won = value;
    }

    public TimeSpan SessionTimeSpan
    {
      get => sessionTimeSpan;
      set => sessionTimeSpan = value;
    }

    public int MurdererId
    {
      get => murdererId;
      set => murdererId = value;
    }

    public List<FinishedParticipantDto> FinishedParticipantDtos
    {
      get => finishedParticipantDtos;
      set => finishedParticipantDtos = value;
    }
  }

  [Serializable]
  public class FinishedParticipantDto
  {
    [SerializeField]
    private UserDto userDto;
    [SerializeField]
    private GameRole gameRole;
    [SerializeField]
    private int coins;
    [SerializeField]
    private int cups;
    public UserDto UserDto
    {
      get => userDto;
      set => userDto = value;
    }

    public GameRole GameRole
    {
      get => gameRole;
      set => gameRole = value;
    }
    public int Coins
    {
      get => coins;
      set => coins = value;
    }

    public int Cups
    {
      get => cups;
      set => cups = value;
    }

  }
}
