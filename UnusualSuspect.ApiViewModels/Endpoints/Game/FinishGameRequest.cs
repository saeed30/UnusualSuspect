using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class FinishGameRequest
  {
    [SerializeField]
    private int gameId;
    [SerializeField]
    private int cardId;

    public int GameId
    {
      get => gameId;
      set => gameId = value;
    }

    public int CardId
    {
      get => cardId;
      set => cardId = value;
    }
  }
}
