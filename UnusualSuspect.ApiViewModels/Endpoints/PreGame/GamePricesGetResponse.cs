using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
  [Serializable]
  public class GamePricesGetResponse
  {
    [SerializeField]
    private int joinNormalGamePriceInCoin;

    public int JoinNormalGamePriceInCoin
    {
      get => joinNormalGamePriceInCoin;
      set => joinNormalGamePriceInCoin = value;
    }
  }
}
