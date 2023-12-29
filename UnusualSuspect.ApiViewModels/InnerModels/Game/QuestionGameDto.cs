using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class QuestionGameDto
  {
    [SerializeField]
    private short questionId;
    [SerializeField]
    private int questionGameId;
    [SerializeField]
    private string questionContent;
    [SerializeField]
    private short turn;

    public short Turn
    {
      get => turn;
      set => turn = value;
    }

    public short QuestionId
    {
      get => questionId;
      set => questionId = value;
    }

    public int QuestionGameId
    {
      get => questionGameId;
      set => questionGameId = value;
    }

    public string QuestionContent
    {
      get => questionContent;
      set => questionContent = value;
    }
  }
}
