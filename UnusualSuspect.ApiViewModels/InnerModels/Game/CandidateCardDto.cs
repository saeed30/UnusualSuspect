using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class CandidateCardDto
  {
    [SerializeField]
    private int userId;
    [SerializeField]
    private int characterCardId;

    public int UserId
    {
      get => userId;
      set => userId = value;
    }

    public int CharacterCardId
    {
      get => characterCardId;
      set => characterCardId = value;
    }
  }
}
