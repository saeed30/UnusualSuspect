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
    private string packageName;

    public string PackageName
    {
      get => packageName;
      set => packageName = value;
    }

    public string PurchaseToken
    {
      get => purchaseToken;
      set => purchaseToken = value;
    }
  }
}
