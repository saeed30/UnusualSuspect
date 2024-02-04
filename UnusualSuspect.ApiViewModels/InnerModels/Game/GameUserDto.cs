using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class GameUserDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private string? username;
    [SerializeField]
    private string? nickName;
    [SerializeField]
    private Guid? documentGuidKey;
    [SerializeField]
    private short? avatarId;

    public short? AvatarId
    {
      get => avatarId;
      set => avatarId = value;
    }

    public Guid? DocumentGuidKey
    {
      get => documentGuidKey;
      set => documentGuidKey = value;
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
