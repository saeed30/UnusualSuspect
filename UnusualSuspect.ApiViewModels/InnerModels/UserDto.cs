using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels
{
  [Serializable]
  public class UserDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private string? username;
    [SerializeField]
    private string? nickName;
    [SerializeField]
    private short? avatarId;

    public short? AvatarId
    {
      get => avatarId;
      set => avatarId = value;
    }
    public int Id
    {
      get => id;
      set => id = value;
    }

    public string? Username
    {
      get => username;
      set => username = value;
    }

    public string? NickName
    {
      get => nickName;
      set => nickName = value;
    }

  }
}
