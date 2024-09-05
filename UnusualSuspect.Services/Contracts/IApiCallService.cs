using UnusualSuspect.ApiViewModels.Endpoints.LocalOnly;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Contracts;

public interface IApiCallService
{
  Task<UnusualSuspectServiceResult<ApiResultCommon>> ChangeGameStateAsync(ChangeGameStateRequest request,
    string username, CancellationToken cancellationToken = default);

  Task<UnusualSuspectServiceResult<(bool, string?)>> CheckPaymentInCafebazaar(PaymentUser paymentUser, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(bool, string?)>> CheckPaymentInMyket(PaymentUser paymentUser, CancellationToken cancellationToken = default);
}