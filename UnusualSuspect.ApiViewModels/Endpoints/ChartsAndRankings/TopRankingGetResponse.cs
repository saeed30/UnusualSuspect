using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings
{
  [Serializable]
  public class TopRankingGetResponse
  {
    [SerializeField]
    private List<RankingTableBaseDto> topDayRanking;
    [SerializeField]
    private List<RankingTableBaseDto> topWeekRanking;
    [SerializeField]
    private List<RankingTableBaseDto> topMonthRanking;
    [SerializeField]
    private List<RankingTableBaseDto> topTotalRanking;

    public List<RankingTableBaseDto> TopDayRanking
    {
      get => topDayRanking;
      set => topDayRanking = value;
    }

    public List<RankingTableBaseDto> TopWeekRanking
    {
      get => topWeekRanking;
      set => topWeekRanking = value;
    }

    public List<RankingTableBaseDto> TopMonthRanking
    {
      get => topMonthRanking;
      set => topMonthRanking = value;
    }

    public List<RankingTableBaseDto> TopTotalRanking
    {
      get => topTotalRanking;
      set => topTotalRanking = value;
    }
  }
}
