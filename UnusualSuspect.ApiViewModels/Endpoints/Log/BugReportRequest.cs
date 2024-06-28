using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Log
{
  [Serializable]
  public class BugReportRequest
  {
    [SerializeField]
    private string userDesc;

    public string UserDesc
    {
      get => userDesc;
      set => userDesc = value;
    }
  }
}
