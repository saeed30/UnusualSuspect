namespace UnusualSuspect.Services.Contracts;

public interface IPaymentUserService
{
  Task CheckAllUncheckedPayments(int? userId = null, CancellationToken cancellationToken = default);
}