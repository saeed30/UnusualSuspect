namespace UnusualSuspect.Services.Contracts;

public interface IPaymentUserService
{
  Task CheckAllUncheckedPayments(CancellationToken cancellationToken);
}