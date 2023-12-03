using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories
{
  public sealed class QuestionRepository(IUnitOfWork uow, ILogger<QuestionRepository> logger)
    : EfRepository<Question, short>(uow, logger), IQuestionRepository
  {
  }
}
