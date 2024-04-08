using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class PrivateInfoDto
  {
    [SerializeField]
    private GameRole gameRole;
    [SerializeField]
    private int murdererId;

    public GameRole GameRole
    {
      get => gameRole;
      set => gameRole = value;
    }

    public int MurdererId
    {
      get => murdererId;
      set => murdererId = value;
    }
  }
}
