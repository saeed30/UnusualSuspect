using System;
using System.Collections.Generic;
using System.Text;

namespace UnusualSuspect.ApiViewModels.Endpoints.Gem
{
  public sealed class GemPurchaseRequest
  {
    public int GemPackageId {
      get;
      set;
    }
    public string PurchaseToken
    {
      get;
      set;
    }
  }
}
