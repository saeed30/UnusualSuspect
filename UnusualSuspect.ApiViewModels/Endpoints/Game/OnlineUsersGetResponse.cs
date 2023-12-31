using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class OnlineUsersGetResponse
  {
    [SerializeField]
    private int gameId;
    [SerializeField]
    private List<int> userIds;

    public int GameId
    {
      get => gameId;
      set => gameId = value;
    }

    public List<int> UserIds
    {
      get => userIds;
      set => userIds = value;
    }
  }
}
