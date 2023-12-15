using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping
{
  public static class PreGameGroupMapper
  {
    public static PreGameGroupGetResponse ToGetPreGameGroupDetailResponse(this PreGameGroup value)
    {
      return new PreGameGroupGetResponse()
      {
        PreGameGroupId = value.Id
      };
    }
    public static IEnumerable<PreGameGroupGetResponse> ToGetPreGameGroupDetailResponse(this IEnumerable<PreGameGroup> value)
    {
      return value.Select(x => x.ToGetPreGameGroupDetailResponse());
    }

  }
}
