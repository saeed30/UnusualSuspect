using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
  public class AddFriendRequest
  {
		[SerializeField]
    private string friendMobileNumber;

    public string FriendMobileNumber
    {
      get => friendMobileNumber;
      set => friendMobileNumber = value;
    }
  }
}
