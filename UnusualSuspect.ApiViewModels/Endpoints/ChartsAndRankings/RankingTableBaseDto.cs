using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings
{
  [Serializable]
  public class RankingTableBaseDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private UserDto userDto;
    [SerializeField]
    private int scoreSum;
    [SerializeField]
    private int rank;

    public int Id
    {
      get => id;
      set => id = value;
    }

    public UserDto UserDto
    {
      get => userDto;
      set => userDto = value;
    }

    public int ScoreSum
    {
      get => scoreSum;
      set => scoreSum = value;
    }

    public int Rank
    {
      get => rank;
      set => rank = value;
    }
  }
}
