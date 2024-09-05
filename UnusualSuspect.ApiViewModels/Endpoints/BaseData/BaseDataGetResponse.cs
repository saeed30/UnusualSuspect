using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.BaseData
{
	[Serializable]
  public class BaseDataGetResponse
  {
    [SerializeField]
    private int version;

    public int Version
    {
      get => version;
      set => version = value;
    }
  }
}
