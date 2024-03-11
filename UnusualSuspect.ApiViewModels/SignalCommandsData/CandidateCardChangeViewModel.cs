using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.SignalCommandsData
{
  [Serializable]
  public class CandidateCardChangeViewModel
  {
    [SerializeField]
    private int userId;
    [SerializeField]
    private int? characterCardId;

    public int UserId
    {
      get => userId;
      set => userId = value;
    }

    public int? CharacterCardId
    {
      get => characterCardId;
      set => characterCardId = value;
    }

  }
}
