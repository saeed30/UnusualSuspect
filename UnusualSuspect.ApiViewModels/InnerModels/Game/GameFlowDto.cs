using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class GameFlowDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private List<short> activeCharacterIds;
    [SerializeField]
    private bool? witnessLastAnswer;
    [SerializeField]
    private TurnOfPlayTalkingState? turnOfPlayTalkingState;

    public bool? WitnessLastAnswer
    {
      get => witnessLastAnswer;
      set => witnessLastAnswer = value;
    }

    public int Id
    {
      get => id;
      set => id = value;
    }
    public List<short> ActiveCharacterIds
    {
      get => activeCharacterIds;
      set => activeCharacterIds = value;
    }
    public TurnOfPlayTalkingState? TurnOfPlayTalkingState
    {
      get => turnOfPlayTalkingState;
      set => turnOfPlayTalkingState = value;
    }

  }
}
