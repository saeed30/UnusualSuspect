using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels.Game
{
  [Serializable]
  public class GameCharacterDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private short characterId;
    [SerializeField]
    private string title;
    [SerializeField]
    private bool isMurderer;
    [SerializeField]
    private string imageUrl;

    public string ImageUrl
    {
      get => imageUrl;
      set => imageUrl = value;
    }

    public int Id
    {
      get => id;
      set => id = value;
    }

    public short CharacterId
    {
      get => characterId;
      set => characterId = value;
    }

    public string Title
    {
      get => title;
      set => title = value;
    }

    public bool IsMurderer
    {
      get => isMurderer;
      set => isMurderer = value;
    }
  }
}
