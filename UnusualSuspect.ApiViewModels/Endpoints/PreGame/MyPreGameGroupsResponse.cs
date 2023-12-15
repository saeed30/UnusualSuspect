using System.Collections.Generic;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
  public sealed class MyPreGameGroupsResponse
  {
    public List<PreGameGroupDto> PreGameGroups { get; set; }
  }
}
