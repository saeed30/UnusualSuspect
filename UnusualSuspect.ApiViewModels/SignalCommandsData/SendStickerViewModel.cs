using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.SignalCommandsData
{
  [Serializable]
  public class SendStickerViewModel
  {
    [SerializeField]
    private int userId;
    [SerializeField]
    private int stickerId;

    public int UserId
    {
      get => userId;
      set => userId = value;
    }

    public int StickerId
    {
      get => stickerId;
      set => stickerId = value;
    }
  }
}
