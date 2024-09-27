using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Gem
{
  [Serializable]
  public class CafeBazaarRequest
  {
    [SerializeField]
    private string purchaseToken;
    [SerializeField]
    private string productId;

    public string ProductId
    {
      get => productId;
      set => productId = value;
    }

    public string PurchaseToken
    {
      get => purchaseToken;
      set => purchaseToken = value;
    }
  }
}
