using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Log
{
	[Serializable]
  public class ErrorRequest
  {
    [SerializeField]
    private string errorContent;
    [SerializeField]
    private List<string> errorParameterList;

    public List<string> ErrorParameterList
    {
      get => errorParameterList;
      set => errorParameterList = value;
    }

    public string ErrorContent
    {
      get => errorContent;
      set => errorContent = value;
    }
  }
}
