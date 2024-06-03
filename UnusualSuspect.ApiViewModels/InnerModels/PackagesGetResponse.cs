using System;
using System.Collections.Generic;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Enums.BaseData;

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
    [SerializeField]
    private PriceTypeEnum? priceTypeEnum;
    [SerializeField]
    private int viewOrder;

    public int ViewOrder
    {
      get => viewOrder;
      set => viewOrder = value;
    }

    public PriceTypeEnum? PriceTypeEnum
    {
      get => priceTypeEnum;
      set => priceTypeEnum = value;
    }

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
