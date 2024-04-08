using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Log
{
	[Serializable]
  public class ErrorRequest
  {
    [SerializeField]
    private string errorContent;

    public string ErrorContent
    {
      get => errorContent;
      set => errorContent = value;
    }
  }
}
