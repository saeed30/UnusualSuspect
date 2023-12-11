using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ApiViewModels.Contracts
{
  public interface IGameHub
  {
    Task SendMessage(string user, string message);
  }
}
