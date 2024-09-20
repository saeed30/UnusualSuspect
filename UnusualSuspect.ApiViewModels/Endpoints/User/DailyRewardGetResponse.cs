using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
  [Serializable]
  public class DailyRewardGetResponse
  {
    [SerializeField]
    private int gemReward;
    [SerializeField]
    private int coinReward;

    public int GemReward
    {
      get => gemReward;
      set => gemReward = value;
    }

    public int CoinReward
    {
      get => coinReward;
      set => coinReward = value;
    }
  }
}
