using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
  [Serializable]
  public class EnterExitSignalingPreGameRequest
  {
    [SerializeField]
    private bool isEntering;
    [SerializeField]
    private string connectionId;
    [SerializeField]
    private int preGameGroupId;

    public bool IsEntering
    {
      get => isEntering;
      set => isEntering = value;
    }

    public string ConnectionId
    {
      get => connectionId;
      set => connectionId = value;
    }

    public int PreGameGroupId
    {
      get => preGameGroupId;
      set => preGameGroupId = value;
    }
  }
}
