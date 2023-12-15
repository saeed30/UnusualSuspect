using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.ApiViewModels.InnerModels.PreGame
{
  public class JoinedPreGameDto
  {
    public DateTime JoinTime { get; set; }
    public bool IsOwnerOfPreGroup { get; set; }
    public int UserId { get; set; }
    public ReadyToGameStatusEnum ReadyToGameStatus { get; set; }
  }
}
