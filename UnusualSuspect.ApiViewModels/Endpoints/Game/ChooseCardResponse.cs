using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
  [Serializable]
  public class ChooseCardResponse
  {
    [SerializeField]
    private bool? wonTheGame;

    public bool? WonTheGame
    {
      get => wonTheGame;
      set => wonTheGame = value;
    }
  }
}
