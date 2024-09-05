using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.ApiViewModels.Endpoints.Gem
{
  [Serializable]
  public sealed class GemPurchaseRequest
  {
    [SerializeField]
    private StoreEnum store;
    [SerializeField]
    private int gemPackageId;
    [SerializeField]
    private string purchaseToken;

    public StoreEnum Store
    {
      get => store;
      set => store = value;
    }

    public int GemPackageId
    {
      get => gemPackageId;
      set => gemPackageId = value;
    }

    public string PurchaseToken
    {
      get => purchaseToken;
      set => purchaseToken = value;
    }
  }
}
