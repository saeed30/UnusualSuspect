using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class WitnessAnswerRequest
  {
    [SerializeField]
    private bool witnessAnswer;
    [SerializeField]
    private short questionId;

    public short QuestionId
    {
	    get => questionId;
	    set => questionId = value;
    }

    public bool WitnessAnswer
    {
      get => witnessAnswer;
      set => witnessAnswer = value;
    }

  }
}
