using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class EnterTheGameRequest
  {
    [SerializeField]
    private string connectionId;
    [SerializeField]
    private int gameId;

    public string ConnectionId
    {
      get => connectionId;
      set => connectionId = value;
    }

    public int GameId
    {
      get => gameId;
      set => gameId = value;
    }
  }
}
