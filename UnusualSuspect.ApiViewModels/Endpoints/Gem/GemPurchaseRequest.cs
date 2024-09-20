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
    private CafeBazaarRequest? cafeBazaarRequest;
    [SerializeField]
    private MyketRequest? myketRequest;

    public CafeBazaarRequest? CafeBazaarRequest
    {
      get => cafeBazaarRequest;
      set => cafeBazaarRequest = value;
    }

    public MyketRequest? MyketRequest
    {
      get => myketRequest;
      set => myketRequest = value;
    }

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

  }
}
