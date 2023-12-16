using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public sealed class CreateGameGroupRequest
	{
    [SerializeField]
    private short gameTypeId;

    public short GameTypeId
    {
      get => gameTypeId;
      set => gameTypeId = value;
    }
  }
}
