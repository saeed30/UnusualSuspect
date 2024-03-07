using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Contracts
{
  public interface IStickerService
  {
    Task<UnusualSuspectServiceResult<bool>> SendStickerToGroupAsync(int userId, short stickerId, int gameId);
  }
}
