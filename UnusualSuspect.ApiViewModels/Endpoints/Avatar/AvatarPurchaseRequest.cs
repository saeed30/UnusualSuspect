using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Avatar
{
  [Serializable]
  public sealed class AvatarPurchaseRequest
  {
    [SerializeField]
    private int avatarPackageId;

    public int AvatarPackageId
    {
      get => avatarPackageId;
      set => avatarPackageId = value;
    }
  }
}
