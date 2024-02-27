using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;

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
    private WitnessAnswer witnessLastAnswer;
    [SerializeField]
    private TurnOfPlayTalkingState? turnOfPlayTalkingState;

    public WitnessAnswer WitnessLastAnswer
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

    //
    public string WitnessAnswerTitle
    {
      get
      {
        switch (WitnessLastAnswer)
        {
          case WitnessAnswer.NoAnswer:
            return "بدون جواب";
          case WitnessAnswer.Yes:
            return "بله";
          case WitnessAnswer.No:
            return "خیر";
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
  }
}
