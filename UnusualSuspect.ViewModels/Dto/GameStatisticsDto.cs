using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Dto;

public class GameStatisticsDto
{
  public int GamesPlayed { get; set; }
  public int GamesWon { get; set; }
  public int GamesLost { get; set; }
}