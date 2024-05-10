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

    [SerializeField]
    private int userDayRanking;
    [SerializeField]
    private int userWeekRanking;
    [SerializeField]
    private int userMonthRanking;
    [SerializeField]
    private int userTotalRanking;
    [SerializeField]
    private int userDayScore;
    [SerializeField]
    private int userWeekScore;
    [SerializeField]
    private int userMonthScore;
    [SerializeField]
    private int userTotalScore;

    public int UserDayRanking
    {
      get => userDayRanking;
      set => userDayRanking = value;
    }

    public int UserWeekRanking
    {
      get => userWeekRanking;
      set => userWeekRanking = value;
    }

    public int UserMonthRanking
    {
      get => userMonthRanking;
      set => userMonthRanking = value;
    }

    public int UserTotalRanking
    {
      get => userTotalRanking;
      set => userTotalRanking = value;
    }

    public int UserDayScore
    {
      get => userDayScore;
      set => userDayScore = value;
    }

    public int UserWeekScore
    {
      get => userWeekScore;
      set => userWeekScore = value;
    }

    public int UserMonthScore
    {
      get => userMonthScore;
      set => userMonthScore = value;
    }

    public int UserTotalScore
    {
      get => userTotalScore;
      set => userTotalScore = value;
    }

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
