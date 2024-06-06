using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
	public sealed class GetProfileInfoResponse
	{
    [SerializeField]
    private int userId;
    [SerializeField]
		private string? nickName;
		[SerializeField]
		private string? userImageDocumentId;
		[SerializeField]
		private int avatarId;
    [SerializeField]
    private bool isMale;
    [SerializeField]
    private int ranking;
    [SerializeField]
    private int gamesPlayed;
    [SerializeField]
    private int gamesWon;
    [SerializeField]
    private int gamesLost;
    [SerializeField]
    private int gems;
    [SerializeField]
    private int coins;
    [SerializeField]
    private int score;

    public int Score
    {
      get => score;
      set => score = value;
    }

    public int Gems
    {
      get => gems;
      set => gems = value;
    }

    public int Coins
    {
      get => coins;
      set => coins = value;
    }

    public int UserId
    {
      get => userId;
      set => userId = value;
    }
    public int GamesPlayed
    {
      get => gamesPlayed;
      set => gamesPlayed = value;
    }

    public int GamesWon
    {
      get => gamesWon;
      set => gamesWon = value;
    }

    public int GamesLost
    {
      get => gamesLost;
      set => gamesLost = value;
    }

    public int Ranking
    {
      get => ranking;
      set => ranking = value;
    }


    public bool IsMale
    {
      get => isMale;
      set => isMale = value;
    }
		public int AvatarId
		{
			get => avatarId;
			set => avatarId = value;
		}

		public string? NickName { get => nickName; set => nickName = value; }
		public string? UserImageDocumentGuidKey { get => userImageDocumentId; set => userImageDocumentId = value; }
	}
}