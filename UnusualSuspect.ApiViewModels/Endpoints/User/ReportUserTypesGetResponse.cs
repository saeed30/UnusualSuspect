using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
  [Serializable]
  public sealed class ReportUserTypesGetResponse
  {
    [SerializeField]
    private List<ReportUserTypeDto> reportUserTypeDtos;

    public List<ReportUserTypeDto> ReportUserTypeDtos
    {
      get => reportUserTypeDtos;
      set => reportUserTypeDtos = value;
    }
  }

  public sealed class ReportUserTypeDto
  {
    [SerializeField]
    private short id;
    [SerializeField]
    private string name;
    [SerializeField]
    private string title;

    public short Id
    {
      get => id;
      set => id = value;
    }

    public string Name
    {
      get => name;
      set => name = value;
    }

    public string Title
    {
      get => title;
      set => title = value;
    }
  }
}
