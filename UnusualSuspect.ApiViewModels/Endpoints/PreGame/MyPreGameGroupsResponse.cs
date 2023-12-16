using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
  public sealed class MyPreGameGroupsResponse
  {
		[SerializeField]
    private List<PreGameGroupDto> preGameGroups;

    public List<PreGameGroupDto> PreGameGroups
    {
      get => preGameGroups;
      set => preGameGroups = value;
    }
  }
}
