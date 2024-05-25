using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.InnerModels
{
  [Serializable]
  public class PackagesGetResponse
  {
    [SerializeField]
    private List<PackageDto> packageDtos;

    public List<PackageDto> PackageDtos
    {
      get => packageDtos;
      set => packageDtos = value;
    }
  }

  [Serializable]
  public sealed class PackageDto
  {
    [SerializeField]
    private int id;
    [SerializeField]
    private int amount;
    [SerializeField]
    private int price;
    [SerializeField]
    private string imageUrl;

    public int Id
    {
      get => id;
      set => id = value;
    }

    public int Amount
    {
      get => amount;
      set => amount = value;
    }

    public int Price
    {
      get => price;
      set => price = value;
    }

    public string ImageUrl
    {
      get => imageUrl;
      set => imageUrl = value;
    }
  }
}
