using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
  public class GetFriendsResponse
  {
		[SerializeField]
    private List<UserDto> friendsUserDto;

    public List<UserDto> FriendsUserDto
    {
      get => friendsUserDto;
      set => friendsUserDto = value;
    }
  }
}
