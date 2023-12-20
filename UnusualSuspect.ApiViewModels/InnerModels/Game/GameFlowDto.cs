using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class GameFlowDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private List<int> activeCharacterIds;

    public int Id
    {
      get => id;
      set => id = value;
    }
    public List<int> ActiveCharacterIds
    {
      get => activeCharacterIds;
      set => activeCharacterIds = value;
    }
  }
}
