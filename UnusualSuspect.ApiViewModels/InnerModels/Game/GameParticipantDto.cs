using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class GameParticipantDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private short orderOfParticipation;
    [SerializeField]
    private UserDto userDto;
    [SerializeField]
    private GameRole gameRole;

    public int Id
    {
      get => id;
      set => id = value;
    }

    public short OrderOfParticipation
    {
      get => orderOfParticipation;
      set => orderOfParticipation = value;
    }

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
  }
}
