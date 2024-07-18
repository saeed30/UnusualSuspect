using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.ApiViewModels.InnerModels.PreGame
{
  [Serializable]
  public class JoinedPreGameDto
  {
    [SerializeField]
    private string joinTimeString;
    [SerializeField]
    private bool isOwnerOfPreGroup;
    [SerializeField]
    private UserDto userDto;
    [SerializeField]
    private ReadyToGameStatusEnum readyToGameStatus;

    public UserDto UserDto
    {
      get => userDto;
      set => userDto = value;
    }

    public string JoinTimeString
    {
      get => joinTimeString;
      set => joinTimeString = value;
    }

    public bool IsOwnerOfPreGroup
    {
      get => isOwnerOfPreGroup;
      set => isOwnerOfPreGroup = value;
    }

    public ReadyToGameStatusEnum ReadyToGameStatus
    {
      get => readyToGameStatus;
      set => readyToGameStatus = value;
    }
  }
}
