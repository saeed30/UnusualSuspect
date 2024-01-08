using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class WitnessAnswerRequest
  {
    [SerializeField]
    private int gameId;
    [SerializeField]
    private bool witnessAnswer;

    public int GameId
    {
      get => gameId;
      set => gameId = value;
    }

    public bool WitnessAnswer
    {
      get => witnessAnswer;
      set => witnessAnswer = value;
    }

  }
}
