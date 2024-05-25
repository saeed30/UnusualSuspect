using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
  public class ReportUserRequest
  {
    [SerializeField]
    private int reportedUserId;
    [SerializeField]
    private string userDescription;
    [SerializeField]
    private int gameId;

    public int ReportedUserId
    {
      get => reportedUserId;
      set => reportedUserId = value;
    }

    public string UserDescription
    {
      get => userDescription;
      set => userDescription = value;
    }

    public int GameId
    {
      get => gameId;
      set => gameId = value;
    }
  }
}
