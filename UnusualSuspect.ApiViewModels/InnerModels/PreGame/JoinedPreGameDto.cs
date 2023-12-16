using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.ApiViewModels.InnerModels.PreGame
{
  [Serializable]
  public class JoinedPreGameDto
  {
    [SerializeField]
    private DateTime joinTime;
    [SerializeField]
    private bool isOwnerOfPreGroup;
    [SerializeField]
    private int userId;
    [SerializeField]
    private ReadyToGameStatusEnum readyToGameStatus;
    [SerializeField]
    private string? nickName;
    [SerializeField]
    private Guid? documentGuidKey;

    public string? NickName
    {
      get => nickName;
      set => nickName = value;
    }

    public Guid? DocumentGuidKey
    {
      get => documentGuidKey;
      set => documentGuidKey = value;
    }

    public DateTime JoinTime
    {
      get => joinTime;
      set => joinTime = value;
    }

    public bool IsOwnerOfPreGroup
    {
      get => isOwnerOfPreGroup;
      set => isOwnerOfPreGroup = value;
    }

    public int UserId
    {
      get => userId;
      set => userId = value;
    }

    public ReadyToGameStatusEnum ReadyToGameStatus
    {
      get => readyToGameStatus;
      set => readyToGameStatus = value;
    }
  }
}
