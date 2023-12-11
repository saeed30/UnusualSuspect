using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Account
{
	[Serializable]
	public sealed class RequestLoginCodeRequest
	{
    [SerializeField]
    private string phoneNumber;

    public string PhoneNumber
    {
      get => phoneNumber;
      set => phoneNumber = value;
    }
  }
}
