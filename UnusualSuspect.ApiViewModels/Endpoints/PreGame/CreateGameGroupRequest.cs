using System;
using System.ComponentModel.DataAnnotations;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InputParameters;

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
