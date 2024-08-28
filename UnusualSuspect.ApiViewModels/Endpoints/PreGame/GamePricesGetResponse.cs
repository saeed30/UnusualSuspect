using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
  [Serializable]
  public class GamePricesGetResponse
  {
    [SerializeField]
    private int joinNormalGamePriceInCoin;
    [SerializeField]
    private int joinNormalGamePriceInCoinForHost;

    public int JoinNormalGamePriceInCoinForHost
    {
      get => joinNormalGamePriceInCoinForHost;
      set => joinNormalGamePriceInCoinForHost = value;
    }

    public int JoinNormalGamePriceInCoin
    {
      get => joinNormalGamePriceInCoin;
      set => joinNormalGamePriceInCoin = value;
    }
  }
}
